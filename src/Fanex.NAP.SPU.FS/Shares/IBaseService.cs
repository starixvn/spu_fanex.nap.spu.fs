namespace Fanex.NAP.SPU.FS.Shares
{
    public interface IBaseService
    {
        IBaseService Precondition();

        IBaseService Rule();

        IBaseService CleanResult();
    }
}
