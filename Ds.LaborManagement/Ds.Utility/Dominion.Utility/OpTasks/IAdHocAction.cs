namespace Dominion.Utility.OpTasks
{
    public interface IAdHocAction<in T>
    {
        void Execute(T obj);
    }

    public interface IAdHocAction<in T1, in T2>
    {
        void Execute(T1 obj1, T2 obj2);
    }

    public interface IAdHocAction<in T1, in T2, in T3>
    {
        void Execute(T1 obj1, T2 obj2, T3 obj3);
    }

    public interface IAdHocAction<in T1, in T2, in T3, in T4>
    {
        void Execute(T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    }
}