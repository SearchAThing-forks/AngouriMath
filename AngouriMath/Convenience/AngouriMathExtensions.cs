
/* Copyright (c) 2019-2020 Angourisoft
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using AngouriMath.Core;
using NumericsComplex = System.Numerics.Complex;
using PeterO.Numbers;
using System.Linq;
using System.Collections.Generic;
using AngouriMath.Core.Exceptions;

namespace AngouriMath.Extensions
{
    using static AngouriMath.Entity.Set;
    using static Entity;
    using static Entity.Number;

    public static partial class AngouriMathExtensions
    {
        /// <summary>
        /// Converts your <see cref="IEnumerable"/> into a set of unique values.
        /// </summary>
        /// <returns>A Set</returns>
        public static FiniteSet ToSet(this IEnumerable<Entity> elements)
            => new FiniteSet(elements);

        /// <summary>
        /// Unites your <see cref="IEnumerable"/> into one <see cref="SetNode"/>.
        /// Applies the "or" operator on those nodes
        /// </summary>
        /// <returns>A set of unique elements</returns>
        public static Set Unite(this IEnumerable<Set> sets)
            => sets.Any() ? sets.Aggregate((a, b) => MathS.Union(a, b)) : Empty;

        /// <summary>
        /// Computes the intersection of your <see cref="IEnumerable"/>'s and makes it one <see cref="SetNode"/>.
        /// Applies the "and" operator on those nodes
        /// </summary>
        /// <returns>A set of unique elements</returns>
        public static Set Intersect(this IEnumerable<Set> sets)
            => sets.Any() ? sets.Aggregate((a, b) => MathS.Intersection(a, b)) : Empty;

        /// <summary>
        /// Parses the expression into <see cref="Entity"/>.
        /// Synonymical to <see cref="MathS.FromString(string)"/>
        /// </summary>
        /// <returns>Expression</returns>
        public static Entity ToEntity(this string expr) => MathS.FromString(expr);

        /// <summary>
        /// Takes a tuple of four and builds an interval
        /// </summary>
        public static Interval ToEntity(this (Entity left, bool leftClosed, Entity right, bool rightClosed) arg)
            => new Interval(arg.left, arg.leftClosed, arg.right, arg.rightClosed);

        /// <summary>
        /// Parses this and simplifies by running <see cref="Entity.Simplify()"/>
        /// </summary>
        /// <returns>Simplified expression</returns>
        public static Entity Simplify(this string expr) => expr.ToEntity().Simplify();

        /// <summary>
        /// Parses this and evals into a number by running <see cref="Entity.EvalNumerical"/>
        /// </summary>
        /// <exception cref="CannotEvalException">
        /// This thrown when the given expression is boolean, tensoric, or contains variables.
        /// First, check whether it can be evaled: <see cref="Entity.EvaluableNumerical"/>
        /// </exception>
        /// <returns>Collapses into one expression</returns>
        public static Complex EvalNumerical(this string expr) => expr.ToEntity().EvalNumerical();

        /// <summary>
        /// Parses this and evals into a boolean by running <see cref="Entity.EvalBoolean"/>
        /// </summary>
        /// <exception cref="CannotEvalException">
        /// This thrown when the given expression is numerical, tensoric, or contains variables.
        /// First, check whether it can be evaled: <see cref="Entity.EvaluableBoolean"/>
        /// </exception>
        /// <returns>Collapses into one expression</returns>
        public static Boolean EvalBoolean(this string expr) => expr.ToEntity().EvalBoolean();

        /// <summary>
        /// Parses and expands the given expression so that as many parentheses as possible
        /// get expanded into a linear expression.
        /// </summary>
        /// <returns>An expanded expression</returns>
        public static Entity Expand(this string expr) => expr.ToEntity().Expand();

        /// <summary>
        /// Parses and factorizes the given expression so that as few powers as possible remain,
        /// and the expression is represented as a product of multipliers
        /// </summary>
        /// <returns>A factorized expression</returns>
        public static Entity Factorize(this string expr) => expr.ToEntity().Factorize();

        /// <summary>
        /// Subsitutes a variable by replacing all its occurances with the given value
        /// </summary>
        /// <param name="var">A variable to substitute</param>
        /// <param name="value">A value to substitute <paramref name="var"/></param>
        /// <returns>Expression with substituted the variable</returns>
        public static Entity Substitute(this string expr, Variable var, Entity value)
            => expr.ToEntity().Substitute(var, value);

        /// <summary>
        /// Solves the given equation
        /// </summary>
        /// <param name="x">The variable to solve over</param>
        /// <returns>A <see cref="SetNode"/> of roots</returns>
        public static Set SolveEquation(this string expr, Variable x)
            => expr.ToEntity().SolveEquation(x);

        /// <summary>
        /// Solves the statement. The given expression must be boolean type,
        /// for example, equality, or boolean operators.
        /// </summary>
        /// <param name="vars">The variables over which to solve</param>
        /// <returns>A <see cref="SetNode"/> of roots</returns>
        public static Set Solve(this string expr, Variable var)
            => expr.ToEntity().Solve(var);

        /// <summary>
        /// Converts an <see cref="int"/> into an AM's understandable <see cref="Integer"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Integer"/></returns>
        public static Integer ToNumber(this int value) => Integer.Create(value);

        /// <summary>
        /// Converts an <see cref="long"/> into an AM's understandable <see cref="Integer"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Integer"/></returns>
        public static Integer ToNumber(this long value) => Integer.Create(value);

        /// <summary>
        /// Converts PeterO's <see cref="EInteger"/> into an AM's understandable <see cref="Integer"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Integer"/></returns>
        public static Integer ToNumber(this EInteger value) => Integer.Create(value);

        /// <summary>
        /// Converts an <see cref="float"/> into an AM's understandable <see cref="Real"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Real"/></returns>
        public static Real ToNumber(this float value) => Real.Create(EDecimal.FromSingle(value));

        /// <summary>
        /// Converts an <see cref="double"/> into an AM's understandable <see cref="Real"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Real"/></returns>
        public static Real ToNumber(this double value) => Real.Create(EDecimal.FromDouble(value));

        /// <summary>
        /// Converts an <see cref="decimal"/> into an AM's understandable <see cref="Real"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Real"/></returns>
        public static Real ToNumber(this decimal value) => Real.Create(EDecimal.FromDecimal(value));

        /// <summary>
        /// Converts PeterO's <see cref="EDecimal"/> into an AM's understandable <see cref="Real"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Real"/></returns>
        public static Real ToNumber(this EDecimal value) => Real.Create(value);

        /// <summary>
        /// Converts Numerics's <see cref="NumericsComplex"/> into an AM's understandable <see cref="Complex"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Complex"/></returns>
        public static Complex ToNumber(this NumericsComplex complex)
            => Complex.Create(EDecimal.FromDouble(complex.Real), EDecimal.FromDouble(complex.Imaginary));

        /// <summary>
        /// Converts an <see cref="bool"/> into an AM's understandable <see cref="Boolean"/>
        /// which can be hung with others
        /// </summary>
        /// <returns>AM's <see cref="Boolean"/></returns>
        public static Boolean ToBoolean(this bool value) => Boolean.Create(value);
        
        /// <summary>
        /// Builds a LaTeX code from an expression
        /// </summary>
        /// <returns>A <see cref="string"/> which can be rendered into pretty output</returns>
        public static string Latexise(this string str) => str.ToEntity().Latexise();

        /// <summary>
        /// Compiles an expression into a special compiled code that runs via
        /// AM's virtual machine. Soon will be deprecated and replaced with compilation to
        /// delegate
        /// </summary>
        /// <param name="variables">The array of variables should cover all variables from the expression</param>
        /// <returns>A compiled expression</returns>
        public static FastExpression Compile(this string str, params Variable[] variables)
            => str.ToEntity().Compile(variables);

        /// <summary>
        /// Finds out the derivative of the given expression 
        /// </summary>
        /// <param name="x">A Variable over which to find a derivative</param>
        /// <returns>A derived expression</returns>
        public static Entity Derive(this string str, Variable x)
            => str.ToEntity().Derive(x);
    }
}