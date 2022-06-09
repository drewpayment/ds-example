namespace Dominion.Utility.OpTasks
{
    public interface IAdHocFunc<in TIn, out TOut>
    {
        TOut Execute(TIn obj);
    }

    public interface IAdHocFunc<in TIn1, in TIn2, out TOut>
    {
        TOut Execute(TIn1 obj1, TIn2 obj2);
    }

    public interface IAdHocFunc<in TIn1, in TIn2, in TIn3, out TOut>
    {
        TOut Execute(TIn1 obj1, TIn2 obj2, TIn3 obj3);
    }
}