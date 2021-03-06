﻿/*
 * Copyright (c) 2019-2020 Angourisoft
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using AngouriMath.Core.Exceptions;
using System.Linq;
using static AngouriMath.Entity;
using static AngouriMath.Entity.Number;
using static AngouriMath.Entity.Set;

namespace AngouriMath.Functions.Algebra.AnalyticalSolving
{
    internal static class AnalyticalInequalitySolver
    {
        /// <summary>
        /// Considers expr > 0
        /// </summary>
        internal static Set Solve(Entity expr, Variable x)
        {
            {
                if (MathS.Utils.TryGetPolyLinear(expr, x, out var a, out var b))
                {
                    a = a.InnerSimplified;
                    b = b.InnerSimplified;
                    var root = PolynomialSolver.SolveLinear(a, b).First();
                    if (root is Complex and not Real)
                        return Empty;
                    if (a is Real { IsNegative: true })
                        return new Interval(Real.NegativeInfinity, false, root, false);
                    return new Interval(root, false, Real.PositiveInfinity, false);
                }
            }
            {
                if (MathS.Utils.TryGetPolyQuadratic(expr, x, out var a, out var b, out var c))
                {
                    a = a.InnerSimplified;
                    b = b.InnerSimplified;
                    c = c.InnerSimplified;
                    var roots = PolynomialSolver.SolveQuadratic(a, b, c);
                    if (roots.Any(c => c is Complex and not Real))
                        return Empty;
                    roots = TreeAnalyzer.SortRealsAndNonReals(roots);
                    var root1 = roots.First();
                    var root2 = roots.Last();
                    if (a is Real { IsNegative: true })
                        return new Interval(root1, false, root2, false);
                    return new Interval(Real.NegativeInfinity, false, root1, false)
                        .Unite(new Interval(root2, false, Real.PositiveInfinity, false));
                }
            }
            throw FutureReleaseException.Raised("Inequalities are not implemented yet", "1.2.1");
        }
    }
}
