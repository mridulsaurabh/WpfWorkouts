using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    /// <summary>
    // A standard double-check algorithm so that you don't lock if the instance has already been created.  
    //  However, because it's possible two threads can go through the first if at the same time the first time back in,
    //  you need to check again after the lock is acquired to avoid creating two instances.
    /// </summary>
    public class HelperUtility
    {
        // static field to hold single instance
        private static volatile HelperUtility _instance = null;
        // lock for thread-safety laziness
        private static readonly object _mutex = new object();

        // private to prevent direct instantiation
        private HelperUtility()
        {

        }

        // Property that does some locking and then creates on first call.        
        public static HelperUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new HelperUtility();
                        }
                    }
                }
                return _instance;
            }
        }

    }


    /// <summary>
    /// Now, while this works perfectly, I hate it.  Why?  
    /// Because it's relying on a non-obvious trick of the IL to guarantee laziness.  
    /// Just looking at this code, you'd have no idea that it's doing what it's doing.  
    /// Worse yet, you may decide that the empty static constructor serves no purpose and delete it (which removes your lazy guarantee).
    /// Worse-worse yet, they may alter the rules around BeforeFieldInit in the future which could change this.
    /// </summary>
    public class Utility
    {
        // because of the static constructor, this won't get created until first use
        private static readonly Utility _utility = new Utility();

        // private to prevent direct instantiation
        private Utility()
        {

        }

        // removes BeforeFieldInit on class so static fields not initialized before they are used.
        static Utility()
        {

        }

        // Returns the singleton instance using lazy-instantiation
        public static Utility UtilityInstance
        {
            get
            {
                return _utility;
            }
        }
    }

    /// <summary>
    /// So, what do i promise instead ? .Net 4.0 adds the System.Lazy type which guarantees thread-safe lazy-construction.
    /// Note, you need your lambda to call the private constructor as Lazy's default constructor can only call public constructors 
    /// of the type passed in (which we can't have by definition of a Singleton). 
    /// But, because the lambda is defined inside our type, it has access to the private members so it's perfect.
    /// </summary>
    public class LazyUtility
    {
        // static holder for instance, need to use lambda to construct since constructor private
        private volatile static Lazy<LazyUtility> _instance = new Lazy<LazyUtility>(() => new LazyUtility());

        // private to prevent direct instantiation
        public LazyUtility()
        {

        }

        public static LazyUtility Utility
        {
            get
            {
                return _instance.Value;
            }
        }
    }

}
