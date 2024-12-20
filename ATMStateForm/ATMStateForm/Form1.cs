using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMStateForm
{
    public partial class Form1 : Form
    {
        private ATMContext atm;

        public Form1()
        {
            InitializeComponent();
            atm = new ATMContext(5000, 1); // Банкомат с 5000 единицами и ID = 1
            UpdateUI();
        }

        private async void btnEnterPin_Click(object sender, EventArgs e)
        {
            int pin;
            if (int.TryParse(txtPin.Text, out pin))
            {
                atm.EnterPIN(pin);
                UpdateUI();

                // Если состояние изменилось на AuthState, скрываем ввод PIN
                if (atm.CurrentState is AuthState)
                {
                    txtPin.Visible = false;
                    btnEnterPin.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Введите корректный PIN.");
            }
        }

        private async void btnWithdraw_Click(object sender, EventArgs e)
        {
            int amount;
            if (int.TryParse(txtAmount.Text, out amount))
            {
                atm.WithdrawAmount(amount);
                UpdateUI();

                // Если состояние изменилось на OperationState, выполняем операцию
                if (atm.CurrentState is OperationState)
                {
                    await PerformOperationAsync();
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму.");
            }
        }

        private async void btnDeposit_Click(object sender, EventArgs e)
        {
            int amount;
            if (int.TryParse(txtAmount.Text, out amount))
            {
                atm.DepositAmount(amount);
                UpdateUI();

                // Если состояние изменилось на OperationState, выполняем операцию
                if (atm.CurrentState is OperationState)
                {
                    await PerformOperationAsync();
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму.");
            }
        }

        private async void btnFinish_Click(object sender, EventArgs e)
        {
            atm.FinishOperation();
            UpdateUI();

            // Если состояние изменилось на OperationState, выполняем операцию
            if (atm.CurrentState is OperationState)
            {
                await PerformOperationAsync();
            }
        }

        private async Task PerformOperationAsync()
        {
            await Task.Delay(1000); 
            atm.SetState(new AuthState()); 
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Расшифровка текущего состояния
            string stateDescription = atm.CurrentState switch
            {
                WaitingState => "Ожидание",
                AuthState => "Аутентификация",
                OperationState => "Выполнение операции",
                BlockedState => "Заблокирован",
                _ => "Неизвестное состояние"
            };

            lblState.Text = $"Текущее состояние: {stateDescription}";
            lblTotalCash.Text = $"Денег в банкомате: {atm.TotalCash}";

            lblNetworkStatus.Text = atm.IsConnectedToNetwork
                ? "Подключение к сети: Успешно"
                : "Подключение к сети: Отсутствует";

            // Проверяем, заблокирован ли банкомат
            bool isBlocked = atm.CurrentState is BlockedState;

            // Скрытие элементов в зависимости от состояния
            bool isAuthenticated = atm.CurrentState is AuthState || atm.CurrentState is OperationState || isBlocked;
            lblTotalCash.Visible = isAuthenticated;
            txtAmount.Visible = isAuthenticated;
            btnWithdraw.Visible = isAuthenticated;
            btnDeposit.Visible = isAuthenticated;
            lblAmount.Visible = isAuthenticated;

            // Установка активности кнопок
            btnWithdraw.Enabled = !isBlocked; // Кнопка снятия становится неактивной, если банкомат заблокирован
            btnDeposit.Enabled = true; // Кнопка пополнения всегда активна

            // Скрытие полей PIN для всех состояний, кроме ожидания
            txtPin.Visible = !isAuthenticated && !isBlocked;
            btnEnterPin.Visible = !isAuthenticated && !isBlocked;
            lblPIN.Visible = !isAuthenticated && !isBlocked;

            // Очищаем поля ввода
            txtPin.Text = "";
            txtAmount.Text = "";
        }
    }

    // Контекст ATM
    public class ATMContext
    {
        public ATMState CurrentState { get; private set; }
        public int TotalCash { get; private set; }
        public int ATMID { get; private set; }
        public bool IsConnectedToNetwork { get; private set; }


        public ATMContext(int initialCash, int id)
        {
            CurrentState = new WaitingState();
            TotalCash = initialCash;
            ATMID = id;

            var random = new Random();
            IsConnectedToNetwork = random.Next(0, 100) > 10;
        }

        public void SetState(ATMState newState) => CurrentState = newState;

        public void UpdateCash(int amount) => TotalCash += amount;

        public void EnterPIN(int pin) => CurrentState.EnterPIN(this, pin);

        public void WithdrawAmount(int amount) => CurrentState.WithdrawAmount(this, amount);

        public void DepositAmount(int amount) => CurrentState.DepositAmount(this, amount);

        public void FinishOperation() => CurrentState.FinishOperation(this);
    }

    // Интерфейс ATMState
    public abstract class ATMState
    {
        public abstract void EnterPIN(ATMContext context, int pin);
        public abstract void WithdrawAmount(ATMContext context, int amount);
        public abstract void DepositAmount(ATMContext context, int amount);
        public abstract void FinishOperation(ATMContext context);
    }

    // Состояния банкомата
    public class WaitingState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin)
        {
            if (pin == 1234)
            {
                MessageBox.Show("Аутентификация успешна.");
                context.SetState(new AuthState());
            }
            else
            {
                MessageBox.Show("Неверный PIN.");
            }
        }

        public override void WithdrawAmount(ATMContext context, int amount) =>
            MessageBox.Show("Сначала необходимо пройти аутентификацию.");

        public override void DepositAmount(ATMContext context, int amount) =>
            MessageBox.Show("Сначала необходимо пройти аутентификацию.");

        public override void FinishOperation(ATMContext context) =>
            MessageBox.Show("Банкомат уже находится в состоянии ожидания.");
    }

    public class AuthState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin)
        {
            MessageBox.Show("Вы уже ввели PIN.");
        }

        public override void WithdrawAmount(ATMContext context, int amount)
        {
            if (amount > context.TotalCash)
            {
                MessageBox.Show("Недостаточно средств в банкомате.");
            }
            else
            {
                context.UpdateCash(-amount);
                MessageBox.Show($"Выдано {amount} единиц. Остаток: {context.TotalCash}.");

                if (context.TotalCash == 0)
                {
                    context.SetState(new BlockedState());
                    MessageBox.Show("Банкомат заблокирован. В нём закончились деньги.");
                }
                else
                {
                    context.SetState(new OperationState());
                }
            }
        }

        public override void DepositAmount(ATMContext context, int amount)
        {
            context.UpdateCash(amount);
            MessageBox.Show($"Добавлено {amount} единиц. Баланс: {context.TotalCash}.");
            context.SetState(new OperationState());
        }

        public override void FinishOperation(ATMContext context)
        {
            MessageBox.Show("Завершение работы.");
            context.SetState(new WaitingState());
        }
    }

    public class OperationState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin) =>
            MessageBox.Show("Нельзя ввести PIN во время операции.");

        public override void WithdrawAmount(ATMContext context, int amount) =>
            MessageBox.Show("Сначала завершите текущую операцию.");

        public override void DepositAmount(ATMContext context, int amount) =>
            MessageBox.Show("Сначала завершите текущую операцию.");

        public override void FinishOperation(ATMContext context)
        {
            MessageBox.Show("Операция завершена. Возвращение к аутентификации.");
            context.SetState(new AuthState());
        }
    }

    public class BlockedState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin)
        {
            MessageBox.Show("Банкомат заблокирован. Обратитесь в службу поддержки.");
        }

        public override void WithdrawAmount(ATMContext context, int amount)
        {
            MessageBox.Show("Банкомат заблокирован. Операции невозможны.");
        }

        public override void DepositAmount(ATMContext context, int amount)
        {
            context.UpdateCash(amount);
            MessageBox.Show($"Добавлено {amount} единиц. Баланс: {context.TotalCash}.");

            // Если после пополнения баланс > 0, переходим в AuthState
            if (context.TotalCash > 0)
            {
                context.SetState(new AuthState());
            }
        }

        public override void FinishOperation(ATMContext context)
        {
            MessageBox.Show("Банкомат заблокирован. Попробуйте позже.");
        }
    }
}
