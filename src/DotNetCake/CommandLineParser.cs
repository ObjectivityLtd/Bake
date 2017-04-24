public class CommandLineParser 
{
    private BuildGenerator buildGenerator = new BuildGenerator();
    public void Parse(string[] args)
    {
        buildGenerator.Generate();
    }

}