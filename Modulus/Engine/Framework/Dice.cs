using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Framework
{
    public class Dice
    {
        static public int Throw(int from, int to)
        {
            return random.Value.Next(from, to);
        }

        static public double Throw(double from, double to)
        {
            return random.Value.NextDouble() * (to - from) + from;
        }

        static public int Throw()
        {
            return random.Value.Next();
        }

        static public int Throw(int max)
        {
            return random.Value.Next(max);
        }

        static private ThreadLocal<Random> random = new ThreadLocal<Random>(() => { return new Random(); });

        public static double ThrowDouble()
        {
            return random.Value.NextDouble();
        }

        public sealed class PopBucket<T> : IBuket<T>
        {

            private List<Tuple<T, int>> orignal = new List<Tuple<T, int>>();
            private ThreadLocal<HashSet<Tuple<T, int>>> picked =
                new ThreadLocal<HashSet<Tuple<T, int>>>(() => { return new HashSet<Tuple<T, int>>(); });
            private ThreadLocal<SortedDictionary<int, Tuple<T, int>>> candidates = new ThreadLocal<SortedDictionary<int, Tuple<T, int>>>(() => { return new SortedDictionary<int, Tuple<T, int>>(); });

            private ThreadLocal<int> MaxPER = new ThreadLocal<int>();

            public void Shuffle()
            {
                picked.Value.Clear();
                candidates.Value.Clear();

                MaxPER.Value = 0;

                //var array = orignal.ToArray();

                //array.Shuffle();

                foreach (var e in orignal)
                {

                    if (e.Item2 == 0) { return; }
                    MaxPER.Value += e.Item2;
                    candidates.Value.Add(MaxPER.Value, e);

                }

            }
            public void Insert(T value, int per)
            {

                if (per == 0) { return; }
                orignal.Add(new Tuple<T, int>(value, per));

                //MaxPER += per;
                //var tuple = new Tuple<T, int>(value, MaxPER);
                //candidates.Value.Add(MaxPER, tuple);

            }

            public void Clear()
            {
                orignal.Clear();
                picked.Value.Clear();
                candidates.Value.Clear();
            }

            public T Pick()
            {

                if (candidates.Value.Count == 0) { return default(T); }
                var dice = Engine.Framework.Dice.Throw(0, MaxPER.Value);
                var pick = candidates.Value.First(e => e.Key >= dice).Value;

                picked.Value.Add(pick);

                candidates.Value.Clear();
                MaxPER.Value = 0;

                foreach (var e in orignal)
                {
                    if (picked.Value.Contains(e) == true) { continue; }
                    MaxPER.Value += e.Item2;
                    candidates.Value.Add(MaxPER.Value, e);
                }

                return pick.Item1;


            }

        }

        public interface IBuket<T>
        {
            void Insert(T value, int per);
            void Shuffle();
            void Clear();
        }

        public sealed class Bucket<T> : IBuket<T>
        {
            public class Slot
            {
                public T Value;
                public int PER;
            }

            private SortedDictionary<int, Tuple<T, int>> items = new SortedDictionary<int, Tuple<T, int>>();
            public int MaxPER { get; set; }

            public void Insert(T value, int per)
            {

                if (per == 0) { return; }
                MaxPER += per;
                var item = new Tuple<T, int>(value, MaxPER);
                items.Add(MaxPER, item);

            }
            public T Pick()
            {

                if (items.Count == 0) { return default(T); }
                var dice = Engine.Framework.Dice.Throw(0, MaxPER);

                var tuple = items.First(e => e.Key >= dice).Value;
                return tuple.Item1;

            }

            public void Shuffle() { }
            public void Clear()
            {
                items.Clear();
            }

        }
    }
}
