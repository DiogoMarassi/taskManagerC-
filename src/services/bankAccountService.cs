

interface ILogger
{
    void Log(string message);
}

// ConsoleLogger imlementa a interface ILogger
class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"Log: {message}");
    }
}

class FileLogger : ILogger
{
    public void Log(string message)
    {
        File.AppendAllText("log.txt", $"{DateTime.Now}: {message}\n");
    }
}

class BankAccount
{
    private string name;
    private readonly ILogger logger;

    // Propriedade
    public decimal Balance
    {
        get;
        private set;
    }

    // Método
    public BankAccount(string name, decimal balance, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        if (balance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(balance), "Balance cannot be negative.");
        }
        this.name = name;
        Balance = balance;
        this.logger = logger;
    }

    // Propriedade
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            logger.Log($"Deposit amount {amount} must be positive.");
            return;
        }
        Balance += amount;
    }
}