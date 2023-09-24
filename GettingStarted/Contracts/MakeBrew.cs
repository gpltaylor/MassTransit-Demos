namespace GettingStarted.Contracts
{
    /**
    * This allows us to make a Cofffee. We infer that a Brew is a Coffee in IT :) 
    */
    public interface IMakeBrew 
    {

        public int Sugar { get; set; }
        public bool Milk { get; set; }
    }

    public interface IMakeBrewCompled
    {

        public int Sugar { get; set; }
        public bool Milk { get; set; }
        public string SomeNewValue { get; set; }
    }

}
