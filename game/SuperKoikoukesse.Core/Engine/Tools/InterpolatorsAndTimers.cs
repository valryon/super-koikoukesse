using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SuperKoikoukesse.Core.Engine.Tools
{
    // The following code is a copy paste from a XNA MVP.
    // Those are very useful tools to make animations, timers, etc.

    /// <summary>
    /// A delegate used by Interpolators to scale their progress and generate their current value.
    /// </summary>
    /// <param name="progress">The current progress of the Interpolator in the range [0, 1].</param>
    /// <returns>A value representing the scaled progress used to generate the Interpolator's Value.</returns>
    public delegate float InterpolatorScaleDelegate(float progress);

    public sealed class Interpolator
    {
        // ReSharper disable StaticFieldInitializersReferesToFieldBelow
        private static readonly Pool<Interpolator> Interpolators = new Pool<Interpolator>(10, i => i._valid)
        {
            // Initialize is invoked whenever we get an instance through New()
            Initialize = i =>
            {
                i._valid = true;
                i.Progress = 0f;
            },
            // Deinitialize is invoked whenever an object is reclaimed during CleanUp()
            Deinitialize = i =>
            {
                i.Tag = null;
                i._scale = null;
                i._step = null;
                i._completed = null;
            }
        };

        // ReSharper restore StaticFieldInitializersReferesToFieldBelow

        private Action<Interpolator> _completed;

        private float _range;
        private InterpolatorScaleDelegate _scale;
        private float _speed;
        private Action<Interpolator> _step;
        private bool _valid;

        /// <summary>
        /// Gets the interpolator's progress in the range of [0, 1].
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// Gets the interpolator's starting value.
        /// </summary>
        public float Start { get; private set; }

        /// <summary>
        /// Gets the interpolator's ending value.
        /// </summary>
        public float End { get; private set; }

        /// <summary>
        /// Gets the interpolator's current value.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Gets or sets some extra data to the timer.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Creates a new Interpolator.
        /// </summary>
        /// <param name="start">The starting value.</param>
        /// <param name="end">The ending value.</param>
        /// <param name="length">The length of time, in seconds, to perform the interpolation.</param>
        /// <param name="step">An optional callback to invoke when the Interpolator is updated.</param>
        /// <param name="completed">An optional callback to invoke when the Interpolator completes.</param>
        /// <returns>The Interpolator instance.</returns>
        public static Interpolator Create(
            float start,
            float end,
            float length,
            Action<Interpolator> step,
            Action<Interpolator> completed)
        {
            return Create(start, end, length, InterpolatorScales.Linear, step, completed);
        }

        /// <summary>
        /// Creates a new Interpolator.
        /// </summary>
        /// <param name="start">The starting value.</param>
        /// <param name="end">The ending value.</param>
        /// <param name="length">The length of time, in seconds, to perform the interpolation.</param>
        /// <param name="scale">A method to perform</param>
        /// <param name="step">An optional callback to invoke when the Interpolator is updated.</param>
        /// <param name="completed">An optional callback to invoke when the Interpolator completes.</param>
        /// <returns>The Interpolator instance.</returns>
        public static Interpolator Create(
            float start,
            float end,
            float length,
            InterpolatorScaleDelegate scale,
            Action<Interpolator> step,
            Action<Interpolator> completed)
        {
            if (length <= 0f)
                throw new ArgumentException("length must be greater than zero");
            if (scale == null)
                throw new ArgumentNullException("scale");

            var i = Interpolators.New();
            i.Start = start;
            i.End = end;
            i._range = end - start;
            i._step = step;
            i._completed = completed;
            i._scale = scale;
            i._speed = 1f / length;

            return i;
        }

        /// <summary>
        /// Stops the Interpolator.
        /// </summary>
        public void Stop()
        {
            _valid = false;
        }

        /// <summary>
        /// Updates all the Interpolators.
        /// </summary>
        /// <param name="dt">The elapsed time (in seconds) to advance the timers. Generally you want to pass in (float)gameTime.ElapsedGameTime.TotalSeconds from your main Game class.</param>
        public static void Update(float dt)
        {
            for (var i = 0; i < Interpolators.ValidCount; i++)
            {
                var p = Interpolators[i];

                // if Stop was called, the interpolator may already be invalid, so we
                // make sure to skip those interpolators.
                if (!p._valid)
                    continue;

                // update the progress, clamping at 1f
                p.Progress = Math.Min(p.Progress + p._speed * dt, 1f);

                // get the scaled progress and use that to generate the value
                var scaledProgress = p._scale(p.Progress);
                p.Value = p.Start + p._range * scaledProgress;

                // invoke the step callback
                if (p._step != null)
                    p._step(p);

                // if the progress is 1...
                if (p.Progress != 1f) continue;
                // the interpolator is done
                p._valid = false;

                // invoke the completed callback
                if (p._completed != null)
                    p._completed(p);
            }

            // clean up the invalid interpolators
            Interpolators.CleanUp();
        }
    }

    /// <summary>
    /// A static class that contains predefined scales for Interpolators.
    /// </summary>
    public static class InterpolatorScales
    {
        /// <summary>
        /// A linear interpolator scale. This is used by default by the Interpolator if no other scale is given.
        /// </summary>
        public static readonly InterpolatorScaleDelegate Linear = LinearInterpolation;

        /// <summary>
        /// A quadratic interpolator scale.
        /// </summary>
        public static readonly InterpolatorScaleDelegate Quadratic = QuadraticInterpolation;

        /// <summary>
        /// A cubic interpolator scale.
        /// </summary>
        public static readonly InterpolatorScaleDelegate Cubic = CubicInterpolation;

        /// <summary>
        /// A quartic interpolator scale.
        /// </summary>
        public static readonly InterpolatorScaleDelegate Quartic = QuarticInterpolation;

        private static float LinearInterpolation(float progress)
        {
            return progress;
        }

        private static float QuadraticInterpolation(float progress)
        {
            return progress * progress;
        }

        private static float CubicInterpolation(float progress)
        {
            return progress * progress * progress;
        }

        private static float QuarticInterpolation(float progress)
        {
            return progress * progress * progress * progress;
        }
    }

    /// <summary>
    /// An object that invokes an action after an amount of time has elapsed and
    /// optionally continues repeating until told to stop.
    /// </summary>
    public sealed class Timer
    {
        // ReSharper disable StaticFieldInitializersReferesToFieldBelow
        private static readonly Pool<Timer> Timers = new Pool<Timer>(10, t => t._valid)
        {
            // Initialize is invoked whenever we get an instance through New()
            Initialize = t =>
            {
                t._valid = true;
                t._time = 0f;
            },
            // Deinitialize is invoked whenever an object is reclaimed during CleanUp()
            Deinitialize = t =>
            {
                t._tick = null;
                t.Tag = null;
            }
        };

        // ReSharper restore StaticFieldInitializersReferesToFieldBelow

        private Action<Timer> _tick;
        private float _time;
        private bool _valid;

        /// <summary>
        /// Gets or sets some extra data to the timer.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets whether or not this timer repeats.
        /// </summary>
        public bool Repeats { get; private set; }

        /// <summary>
        /// Gets the length of time (in seconds) between ticks of the timer.
        /// </summary>
        public float TickLength { get; private set; }

        /// <summary>
        /// Creates a new Timer.
        /// </summary>
        /// <param name="tickLength">The amount of time between the timer's ticks.</param>
        /// <param name="repeats">Whether or not the timer repeats.</param>
        /// <param name="tick">An action to perform when the timer ticks.</param>
        /// <returns>The new Timer object or null if the timer pool is full.</returns>
        public static Timer Create(float tickLength, bool repeats, Action<Timer> tick)
        {
            if (tickLength <= 0f)
                throw new ArgumentException("tickLength must be greater than zero.");
            if (tick == null)
                throw new ArgumentNullException("tick");

            // get a new timer from the pool
            var t = Timers.New();
            t.TickLength = tickLength;
            t.Repeats = repeats;
            t._tick = tick;

            return t;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            _valid = false;
        }

        /// <summary>
        /// Updates all the Timers.
        /// </summary>
        /// <param name="dt">The elapsed time (in seconds) to advance the timers. Generally you want to pass in (float)gameTime.ElapsedGameTime.TotalSeconds from your main Game class.</param>
        public static void Update(float dt)
        {
            for (var i = 0; i < Timers.ValidCount; i++)
            {
                var t = Timers[i];

                // if a timer is stopped manually, it may not
                // be valid at this point so we skip it.
                if (!t._valid)
                    continue;

                // update the timer's time
                t._time += dt;

                // if the timer passed its tick length...
                if (t._time < t.TickLength) continue;
                // perform the action
                t._tick(t);

                // subtract the tick length in case we need to repeat
                t._time -= t.TickLength;

                // if the timer doesn't repeat, it is no longer valid
                if (!t.Repeats)
                    t._valid = false;
            }

            // clean up any invalid timers
            Timers.CleanUp();
        }
    }

    /// <summary>
    /// A collection that maintains a set of class instances to allow for recycling
    /// instances and minimizing the effects of garbage collection.
    /// </summary>
    /// <typeparam name="T">The type of object to store in the Pool. Pools can only hold class types.</typeparam>
    public class Pool<T> where T : class
    {
        // the amount to enlarge the items array if New is called and there are no free items

        #region Delegates

        /// <summary>
        /// A delegate that returns a new object instance for the Pool.
        /// </summary>
        /// <returns>A new object instance.</returns>
        public delegate T Allocate();

        #endregion

        private const int ResizeAmount = 20;

        // the actual items of the pool

        // used for allocating instances of the object
        private readonly Allocate _allocate;

        // a constructor the default allocate method can use to create instances
        private readonly ConstructorInfo _constructor;
        private readonly Predicate<T> _validate;
        private T[] _items;

        /// <summary>
        /// Creates a new pool.
        /// </summary>
        /// <param name="validateFunc">A predicate used to determine if a given object is still valid.</param>
        public Pool(Predicate<T> validateFunc)
            : this(0, validateFunc)
        {
        }

        /// <summary>
        /// Creates a new pool with a specific starting size.
        /// </summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="validateFunc">A predicate used to determine if a given object is still valid.</param>
        public Pool(int initialSize, Predicate<T> validateFunc)
            : this(initialSize, validateFunc, null)
        {
        }

        /// <summary>
        /// Creates a new pool with a specific starting size.
        /// </summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="validateFunc">A predicate used to determine if a given object is still valid.</param>
        /// <param name="allocateFunc">A function used to allocate an instance for the pool.</param>
        public Pool(int initialSize, Predicate<T> validateFunc, Allocate allocateFunc)
        {
            // validate some parameters
            if (initialSize < 0)
                throw new ArgumentException("initialSize must be non-negative");
            if (validateFunc == null)
                throw new ArgumentNullException("validateFunc");

            if (initialSize == 0)
                initialSize = 10;

            _items = new T[initialSize];
            _validate = validateFunc;
            InvalidCount = _items.Length;

            // default to using a parameterless constructor if no allocateFunc was given
            _allocate = allocateFunc ?? ConstructorAllocate;

            // if we are using the ConstructorAllocate method, make sure we have a valid parameterless constructor
            if (_allocate != ConstructorAllocate)
            {
            }
            else
            {
                // we want to find any parameterless constructor, public or not
                _constructor = typeof(T).GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] { },
                    null);

                if (_constructor == null)
                    throw new InvalidOperationException(typeof(T) + " does not have a parameterless constructor.");
            }
        }

        /// <summary>
        /// Gets or sets a delegate used for initializing objects before returning them from the New method.
        /// </summary>
        public Action<T> Initialize { get; set; }

        /// <summary>
        /// Gets or sets a delegate that is run when an object is moved from being valid to invalid
        /// in the CleanUp method.
        /// </summary>
        public Action<T> Deinitialize { get; set; }

        /// <summary>
        /// Gets the number of valid objects in the pool.
        /// </summary>
        public int ValidCount
        {
            get { return _items.Length - InvalidCount; }
        }

        /// <summary>
        /// Gets the number of invalid objects in the pool.
        /// </summary>
        public int InvalidCount { get; private set; }

        /// <summary>
        /// Returns a valid object at the given index. The index must fall in the range of [0, ValidCount].
        /// </summary>
        /// <param name="index">The index of the valid object to get</param>
        /// <returns>A valid object found at the index</returns>
        public T this[int index]
        {
            get
            {
                index += InvalidCount;

                if (index < InvalidCount || index >= _items.Length)
                    throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount");

                return _items[index];
            }
        }

        /// <summary>
        /// Cleans up the pool by checking each valid object to ensure it is still actually valid.
        /// </summary>
        public void CleanUp()
        {
            for (var i = InvalidCount; i < _items.Length; i++)
            {
                var obj = _items[i];

                // if it's still valid, keep going
                if (_validate(obj))
                    continue;

                // otherwise if we're not at the start of the invalid objects, we have to move
                // the object to the invalid object section of the array
                if (i != InvalidCount)
                {
                    _items[i] = _items[InvalidCount];
                    _items[InvalidCount] = obj;
                }

                // clean the object if desired
                if (Deinitialize != null)
                    Deinitialize(obj);

                InvalidCount++;
            }
        }

        /// <summary>
        /// Returns a new object from the Pool.
        /// </summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        public T New()
        {
            // if we're out of invalid instances, resize to fit some more
            if (InvalidCount == 0)
            {
#if DEBUG && WINDOWS
                Trace.WriteLine("Resizing pool. Old size: " + _items.Length + ". New size: " +
                                (_items.Length + ResizeAmount));
#endif
                // create a new array with some more slots and copy over the existing items
                var newItems = new T[_items.Length + ResizeAmount];
                for (var i = _items.Length - 1; i >= 0; i--)
                    newItems[i + ResizeAmount] = _items[i];
                _items = newItems;

                // move the invalid count based on our resize amount
                InvalidCount += ResizeAmount;
            }

            // decrement the counter
            InvalidCount--;

            // get the next item in the list
            var obj = _items[InvalidCount];

            // if the item is null, we need to allocate a new instance
            if (obj == null)
            {
                obj = _allocate();

                if (obj == null)
                    throw new InvalidOperationException("The pool's allocate method returned a null object reference.");

                _items[InvalidCount] = obj;
            }

            // initialize the object if a delegate was provided
            if (Initialize != null)
                Initialize(obj);

            return obj;
        }

        // a default Allocate delegate for use when no custom allocate delegate is provided
        private T ConstructorAllocate()
        {
            return _constructor.Invoke(null) as T;
        }
    }
}
