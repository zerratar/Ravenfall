namespace Shinobytes.Ravenfall.DataModels
{
    public class ExpSkillGainCollection
    {
        public ExpGain Attack { get; set; } = new ExpGain();
        public ExpGain Defense { get; set; } = new ExpGain();
        public ExpGain Strength { get; set; } = new ExpGain();
        public ExpGain Health { get; set; } = new ExpGain();
        public ExpGain Woodcutting { get; set; } = new ExpGain();
        public ExpGain Fishing { get; set; } = new ExpGain();
        public ExpGain Mining { get; set; } = new ExpGain();
        public ExpGain Crafting { get; set; } = new ExpGain();
        public ExpGain Cooking { get; set; } = new ExpGain();
        public ExpGain Farming { get; set; } = new ExpGain();
        public ExpGain Slayer { get; set; } = new ExpGain();
        public ExpGain Magic { get; set; } = new ExpGain();
        public ExpGain Ranged { get; set; } = new ExpGain();
        public ExpGain Sailing { get; set; } = new ExpGain();
    }
}