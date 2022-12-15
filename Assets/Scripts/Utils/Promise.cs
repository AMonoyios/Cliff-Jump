using System;
using System.Collections.Generic;

public class Promise<T>
{
    private bool finishedExecuting;
    private Func<T, bool> condition;
    private readonly List<Func<T>> promise = new();

    public Promise<T> Add(Func<T> newPromise)
    {
        promise.Add(newPromise);
        return this;
    }

    public Promise<T> Condition(Func<T, bool> condition)
    {
        this.condition = condition;
        return this;
    }

    private bool CanExecute
    {
        get { return promise != null && promise.Count != 0 && condition != null; }
    }

    public Promise<T> Execute()
    {
        finishedExecuting = false;

        if (CanExecute)
        {
            for (int i = 0; i < promise.Count; i++)
            {
                if (promise[i] == null)
                {
                    throw new Exception("Null promise detected!");
                }
                else
                {
                    if (!condition(promise[i].Invoke()))
                    {
                        throw new Exception("Condition promise failed!");
                    }
                }
            }

            finishedExecuting = true;
        }
        else
        {
            throw new Exception("Failed to Execute promise!");
        }

        return this;
    }

    public Promise<T> OnComplete(Action onComplete)
    {
        if (CanExecute && finishedExecuting)
        {
            if (onComplete == null)
            {
                throw new Exception("Null onComplete detected!");
            }
            else
            {
                onComplete.Invoke();

                return this;
            }
        }
        else
        {
            throw new Exception("Execution is required!");
        }
    }
}
