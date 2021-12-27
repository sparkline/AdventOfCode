using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day24 : Solver
    {
        public Day24() : base(2021, 24) { }



        protected override object PartA(string input)
        {
            int[] maxNumber = new int[14];
            var data = ReadVars(input);
            foreach ((int algNo, int pushedAlgNo, int RHS) item in data)
            {
                // Make every push after a pop fail, since the pop is cleaning up the mess off the previous instruction, but should leave a z = 0
                // So we need this to be true: input == (z.pop() + var1) == (prevInput + prevVar2 + var1) == prevInput + RHS == Input
                // Maximize, so get them close to 9. If RHS is positive, and input can be max 9, then prevInput = 9-RHS
                if (item.RHS > 0)
                {
                    // 9 + 1 is not a valid input, remember:  (input != (z.pop() + VAR1))
                    maxNumber[item.algNo] = 9;
                    maxNumber[item.pushedAlgNo] = 9 - item.RHS;
                } else
                {
                    maxNumber[item.algNo] = 9 + item.RHS;
                    maxNumber[item.pushedAlgNo] = 9;
                }
            }
            var answer = string.Join("", maxNumber);
            return long.Parse(answer);

        }

        private List<(int algNo, int pushedAlgNo, int RHS)> ReadVars(string input)
        {
            Stack<(int algNo, int var2)> s = new Stack<(int algNo, int var2)>();
            List<(int algNo, int pushedAlgNo, int RHS)> result = new List<(int algNo, int pushedAlgNo, int RHS)>();
            var lines = input.SplitOnNewline();
            for (int algNo = 0;algNo<14;algNo++)
            {
                bool doPop = lines[algNo * 18 + 4].Split(' ')[2]=="26";
                int var1 = int.Parse(lines[algNo * 18 + 5].Split(' ')[2]);
                int var2 = int.Parse(lines[algNo * 18 + 15].Split(' ')[2]);
                if (doPop)
                {
                    // if (input != (z.pop() + VAR1)
                    // z.push(VAR2)
                    // therefore RHS = z.pop()+VAR1
                    (int pushedAlgNo, int pushedVar2) zz = s.Pop();
                    result.Add((algNo, zz.pushedAlgNo, var1 + zz.pushedVar2));
                } 
                else
                {
                    s.Push((algNo, var2));
                }
            }
            return result;
        }

        protected override object PartB(string input)
        {
            int[] maxNumber = new int[14];
            var data = ReadVars(input);
            foreach ((int algNo, int pushedAlgNo, int RHS) item in data)
            {
                // Make every push after a pop fail, since the pop is cleaning up the mess off the previous instruction, but should leave a z = 0
                // So we need this to be true: input == (z.pop() + var1) == (prevInput + prevVar2 + var1) == prevInput + RHS == Input
                // Minimize, so get them close to 1. If RHS is positive, and input can be min 1, then Input = 1 + RHS
                if (item.RHS > 0)
                {
                    maxNumber[item.algNo] = 1 + item.RHS;
                    maxNumber[item.pushedAlgNo] = 1;
                }
                else
                {
                    maxNumber[item.algNo] = 1;
                    maxNumber[item.pushedAlgNo] = 1 - item.RHS;
                }
            }
            var answer = string.Join("", maxNumber);
            return long.Parse(answer);
        }

        /*
         * inp w            input 1-9
         * mul x 0      
         * add x z      
         * mod x 26         x = z % 26
         * div z 1 OR 26    26 == z.pop     
         * add x 12         x += VAR1
         * eql x w          x == input
         * eql x 0          x == ((z % 26 + VAR1) != input)
         * mul y 0          
         * add y 25         
         * mul y x          
         * add y 1          y == 26 OR 1 (depending on x)
         * mul z y          if ((z % 26 + VAR1) != input)) z.push A
         * mul y 0
         * add y w          
         * add y 1          VAR2
         * mul y x          
         * add z y          A = input+VAR2
         * 
         * if (input != (z.pop() + VAR1)
         *  z.push(VAR2)
         * 
         */


        /* Magic with functions, never gonna work in time.
        Func<State, ID, VAR, State> inp = (state, id1, value) => Assign(state, id1, value.Value(state));
        Func<State, ID, VAR, State> add = (state, id1, value) => Assign(state, id1, Value(state, id1) + value.Value(state));
        Func<State, ID, VAR, State> mul = (state, id1, value) => Assign(state, id1, Value(state, id1) * value.Value(state));
        Func<State, ID, VAR, State> div = (state, id1, value) => Assign(state, id1, Value(state, id1) / value.Value(state));
        Func<State, ID, VAR, State> mod = (state, id1, value) => Assign(state, id1, Value(state, id1) % value.Value(state));
        Func<State, ID, VAR, State> eql = (state, id1, value) => Assign(state, id1, Value(state, id1) == value.Value(state) ? 1 : 0);

        List<List<(Func<State, ID, VAR, State> func, ID id1, VAR value)>> functions;
        Dictionary<(int w, int z), long> cache = new Dictionary<(int w, int z), long> ();
        
        
        private string MaxInput(int funcDepth, HashSet<int> zIn)
        {
            var alg = functions[funcDepth];
            HashSet<int> zOut = new HashSet<int>();
            var functionList = functions[funcDepth];
            for (int w = 9; w >= 1; w--)
            {
                //                Console.Write(w+"");
                zOut.Clear();
                foreach (int z in zIn)
                {
                    int zz;
                    //                  if (!cache.TryGetValue((w, z, funcDepth), out zz))
                    //                  {
                    State s = new State(w, z);
                    VAR inp = new VAR(w);
                    var bootstrapAlgo = alg.First();
                    s = bootstrapAlgo.func(s, bootstrapAlgo.id1, inp);
                    foreach (var a in alg.Skip(1))
                    {
                        s = a.func(s, a.id1, a.value);
                    }
                    zz = s.z;
                    //                        cache.Add((w, z, funcDepth),zz);
                    //                    }
                    zOut.Add(zz);
                }

                if (funcDepth == functions.Count - 1)
                {
                    //                    Console.WriteLine();
                    // z == 0?
                    if (zOut.Any(z => z == 0))
                    {
                        return "" + w;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    // recursive call
                    string result = MaxInput(funcDepth + 1, zOut);
                    if (result != null)
                    {
                        return w + result;
                    }
                }


            }
            return null;

        }
        



        protected override object PartA(string input)
        {
            // Read algorithms
            functions = readInput(input);
            StartSW();

            //string maxInput = MaxInput(0, new HashSet<long> { 0L });

            //return long.Parse(maxInput);

            

            // HaCk the algorithm, w in z out. z is base 26
            // Make a dictionary for each w/z combo 
            List<Dictionary<(int w, int z), int>> combos = new List<Dictionary<(int w, int z), int>>();
            HashSet<int> zIn = new HashSet<int> { 0 };
            foreach (var alg in functions)
            {
                HashSet<int> zOut = new HashSet<int> { };
                var dict = new Dictionary<(int w, int z), int>();
                for (int valueForW = 1; valueForW <= 9; valueForW++)
                {
                    foreach (int valueForZ in zIn)
                    {
                        State s = new State(valueForW, valueForZ);
                        VAR inp = new VAR(valueForW);
                        var bootstrapAlgo = alg.First();
                        s = bootstrapAlgo.func(s, bootstrapAlgo.id1, inp);
                        foreach (var a in alg.Skip(1))
                        {
                            s = a.func(s, a.id1, a.value);
                        }

                        long zz = s.z % 26;
                        dict.Add((valueForW, valueForZ), (int)zz);
                        zOut.Add((int)zz); // Could do something with modulo and divide 26 for Z
                    }
                }

                combos.Add(dict);
                zIn = zOut;
            }

            // Search top to bottom
            string maxAnswer = FindMax(ref combos);

            // Step X

            // Profit
            long solution = long.Parse(maxAnswer);
            return solution;
            
        }

        private string FindMax(ref List<Dictionary<(int w, int z), int>> combos, int zIn = 0, int depth = 0)
        {
            for (int w = 9; w >= 1; w--)
            {
                if (combos[depth].TryGetValue((w, zIn), out int zOut))
                {
                    if (depth == combos.Count - 1)
                    {
                        bool ALUisHappy = (zOut == 0) ;
                        return ALUisHappy ? "" + w : "";
                    }
                    else
                    {
                        string result = FindMax(ref combos, zOut, depth + 1);
                        if (!string.IsNullOrEmpty(result))
                        {
                            return w + result;
                        }
                    }
                }
            }
            return "";
        }

        private List<List<(Func<State, ID, VAR, State> func, ID id1, VAR value)>> readInput(string input)
        {

            var result = new List<List<(Func<State, ID, VAR, State>, ID, VAR)>>();
            var lines = input.SplitOnNewline();
            var subRoutine = new List<(Func<State, ID, VAR, State>, ID, VAR)>();
            ID id1;
            VAR id2;
            foreach (var line in lines)
            {
                var s = line.Split(' ');
                switch (s[0])
                {
                    case "inp":
                        if (subRoutine.Count > 0)
                        {
                            result.Add(subRoutine);
                        }
                        subRoutine = new List<(Func<State, ID, VAR, State>, ID, VAR)>();
                        id1 = GetID(s[1]);
                        subRoutine.Add((inp, id1, new VAR(ID.TBT)));
                        break;
                    case "add":
                        id1 = GetID(s[1]);
                        id2 = GetVAR(s[2]);
                        subRoutine.Add((add, id1, id2));
                        break;
                    case "mul":
                        id1 = GetID(s[1]);
                        id2 = GetVAR(s[2]);
                        subRoutine.Add((mul, id1, id2));
                        break;
                    case "div":
                        id1 = GetID(s[1]);
                        id2 = GetVAR(s[2]);
                        subRoutine.Add((div, id1, id2));
                        break;
                    case "mod":
                        id1 = GetID(s[1]);
                        id2 = GetVAR(s[2]);
                        subRoutine.Add((mod, id1, id2));
                        break;
                    case "eql":
                        id1 = GetID(s[1]);
                        id2 = GetVAR(s[2]);
                        subRoutine.Add((eql, id1, id2));
                        break;
                }
            }
            result.Add(subRoutine);

            return result;
        }

        private VAR GetVAR(string v)
        {
            return v switch
            {
                "w" => new VAR(ID.w),
                "x" => new VAR(ID.x),
                "y" => new VAR(ID.y),
                "z" => new VAR(ID.z),
                _ => new VAR(long.Parse(v))
            };
        }

        private ID GetID(string id)
        {
            // Shady =)
            return id switch
            {
                "w" => ID.w,
                "x" => ID.x,
                "y" => ID.y,
                "z" => ID.z,
            };
        }

        private static State Assign(State state, ID id, long value)
        {
            switch (id)
            {
                case ID.w:
                    state.w = value;
                    break;
                case ID.x:
                    state.x = value;
                    break;
                case ID.y:
                    state.y = value;
                    break;
                case ID.z:
                    state.z = value;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return state;
        }

        private static long Value(State state, ID id)
        {
            return id switch
            {
                ID.w => state.w,
                ID.x => state.x,
                ID.y => state.y,
                ID.z => state.z,
            };
        }

        struct VAR
        {
            ID id;
            long v;

            public VAR(ID id) : this()
            {
                this.id = id;
            }
            public VAR(long value) : this()
            {
                this.v = value;
                this.id = ID.TBT;
            }

            public long Value(State state)
            {
                return id switch {
                    ID.w => state.w,
                    ID.x => state.x,
                    ID.y => state.y,
                    ID.z => state.z,
                    _ => v,
                };
            }


        }

        enum ID
        {
            w,
            x,
            y,
            z,
            TBT,
        }

        /*
        struct B26
        {
            public int minor { get; private set; }
            public int major { get; private set; }
            public int value { get { return minor + (26 * major); } set { minor = value % 26; major = value / 26; } }

            public B26(int value)
            {
                minor = value % 26;
                major = value / 26;
            }

            public B26(int minor, int major)
            {
                this.minor = minor;
                this.major = major;
            }

            public override string ToString()
            {
                return value.ToString();
            }
        }
        

        struct State
        {
            public long w, x, y, z;

            public State(long w, long x, long y, long z)
            {
                this.w = w;
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public State(long w, long z) : this(w, 0, 0, z)
            {
            }

            public override string ToString()
            {
                return $"{w,2},{x,2},{y,2},{z,2}";
            }
        }
        */

    }
}
