namespace Slask.Domain
{
    public interface SortableInterface
    {
        public int SortOrder { get; }

        public void UpdateSortOrder();
    }
}
