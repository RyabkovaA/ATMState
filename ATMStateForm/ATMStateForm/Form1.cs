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
            atm = new ATMContext(5000, 1); // �������� � 5000 ��������� � ID = 1
            UpdateUI();
        }

        private async void btnEnterPin_Click(object sender, EventArgs e)
        {
            int pin;
            if (int.TryParse(txtPin.Text, out pin))
            {
                atm.EnterPIN(pin);
                UpdateUI();

                // ���� ��������� ���������� �� AuthState, �������� ���� PIN
                if (atm.CurrentState is AuthState)
                {
                    txtPin.Visible = false;
                    btnEnterPin.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("������� ���������� PIN.");
            }
        }

        private async void btnWithdraw_Click(object sender, EventArgs e)
        {
            int amount;
            if (int.TryParse(txtAmount.Text, out amount))
            {
                atm.WithdrawAmount(amount);
                UpdateUI();

                // ���� ��������� ���������� �� OperationState, ��������� ��������
                if (atm.CurrentState is OperationState)
                {
                    await PerformOperationAsync();
                }
            }
            else
            {
                MessageBox.Show("������� ���������� �����.");
            }
        }

        private async void btnDeposit_Click(object sender, EventArgs e)
        {
            int amount;
            if (int.TryParse(txtAmount.Text, out amount))
            {
                atm.DepositAmount(amount);
                UpdateUI();

                // ���� ��������� ���������� �� OperationState, ��������� ��������
                if (atm.CurrentState is OperationState)
                {
                    await PerformOperationAsync();
                }
            }
            else
            {
                MessageBox.Show("������� ���������� �����.");
            }
        }

        private async void btnFinish_Click(object sender, EventArgs e)
        {
            atm.FinishOperation();
            UpdateUI();

            // ���� ��������� ���������� �� OperationState, ��������� ��������
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
            // ����������� �������� ���������
            string stateDescription = atm.CurrentState switch
            {
                WaitingState => "��������",
                AuthState => "��������������",
                OperationState => "���������� ��������",
                BlockedState => "������������",
                _ => "����������� ���������"
            };

            lblState.Text = $"������� ���������: {stateDescription}";
            lblTotalCash.Text = $"����� � ���������: {atm.TotalCash}";

            lblNetworkStatus.Text = atm.IsConnectedToNetwork
                ? "����������� � ����: �������"
                : "����������� � ����: �����������";

            // ���������, ������������ �� ��������
            bool isBlocked = atm.CurrentState is BlockedState;

            // ������� ��������� � ����������� �� ���������
            bool isAuthenticated = atm.CurrentState is AuthState || atm.CurrentState is OperationState || isBlocked;
            lblTotalCash.Visible = isAuthenticated;
            txtAmount.Visible = isAuthenticated;
            btnWithdraw.Visible = isAuthenticated;
            btnDeposit.Visible = isAuthenticated;
            lblAmount.Visible = isAuthenticated;

            // ��������� ���������� ������
            btnWithdraw.Enabled = !isBlocked; // ������ ������ ���������� ����������, ���� �������� ������������
            btnDeposit.Enabled = true; // ������ ���������� ������ �������

            // ������� ����� PIN ��� ���� ���������, ����� ��������
            txtPin.Visible = !isAuthenticated && !isBlocked;
            btnEnterPin.Visible = !isAuthenticated && !isBlocked;
            lblPIN.Visible = !isAuthenticated && !isBlocked;

            // ������� ���� �����
            txtPin.Text = "";
            txtAmount.Text = "";
        }
    }

    // �������� ATM
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

    // ��������� ATMState
    public abstract class ATMState
    {
        public abstract void EnterPIN(ATMContext context, int pin);
        public abstract void WithdrawAmount(ATMContext context, int amount);
        public abstract void DepositAmount(ATMContext context, int amount);
        public abstract void FinishOperation(ATMContext context);
    }

    // ��������� ���������
    public class WaitingState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin)
        {
            if (pin == 1234)
            {
                MessageBox.Show("�������������� �������.");
                context.SetState(new AuthState());
            }
            else
            {
                MessageBox.Show("�������� PIN.");
            }
        }

        public override void WithdrawAmount(ATMContext context, int amount) =>
            MessageBox.Show("������� ���������� ������ ��������������.");

        public override void DepositAmount(ATMContext context, int amount) =>
            MessageBox.Show("������� ���������� ������ ��������������.");

        public override void FinishOperation(ATMContext context) =>
            MessageBox.Show("�������� ��� ��������� � ��������� ��������.");
    }

    public class AuthState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin)
        {
            MessageBox.Show("�� ��� ����� PIN.");
        }

        public override void WithdrawAmount(ATMContext context, int amount)
        {
            if (amount > context.TotalCash)
            {
                MessageBox.Show("������������ ������� � ���������.");
            }
            else
            {
                context.UpdateCash(-amount);
                MessageBox.Show($"������ {amount} ������. �������: {context.TotalCash}.");

                if (context.TotalCash == 0)
                {
                    context.SetState(new BlockedState());
                    MessageBox.Show("�������� ������������. � �� ����������� ������.");
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
            MessageBox.Show($"��������� {amount} ������. ������: {context.TotalCash}.");
            context.SetState(new OperationState());
        }

        public override void FinishOperation(ATMContext context)
        {
            MessageBox.Show("���������� ������.");
            context.SetState(new WaitingState());
        }
    }

    public class OperationState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin) =>
            MessageBox.Show("������ ������ PIN �� ����� ��������.");

        public override void WithdrawAmount(ATMContext context, int amount) =>
            MessageBox.Show("������� ��������� ������� ��������.");

        public override void DepositAmount(ATMContext context, int amount) =>
            MessageBox.Show("������� ��������� ������� ��������.");

        public override void FinishOperation(ATMContext context)
        {
            MessageBox.Show("�������� ���������. ����������� � ��������������.");
            context.SetState(new AuthState());
        }
    }

    public class BlockedState : ATMState
    {
        public override void EnterPIN(ATMContext context, int pin)
        {
            MessageBox.Show("�������� ������������. ���������� � ������ ���������.");
        }

        public override void WithdrawAmount(ATMContext context, int amount)
        {
            MessageBox.Show("�������� ������������. �������� ����������.");
        }

        public override void DepositAmount(ATMContext context, int amount)
        {
            context.UpdateCash(amount);
            MessageBox.Show($"��������� {amount} ������. ������: {context.TotalCash}.");

            // ���� ����� ���������� ������ > 0, ��������� � AuthState
            if (context.TotalCash > 0)
            {
                context.SetState(new AuthState());
            }
        }

        public override void FinishOperation(ATMContext context)
        {
            MessageBox.Show("�������� ������������. ���������� �����.");
        }
    }
}
