/*
 * Copyright (c) 2010 Ariën Holthuizen

 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:

 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BayesClassifier
{
    public class Category : IEnumerable<KeyValuePair<string, int>>
    {
        private readonly Dictionary<string, int> _phrases;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            return _phrases.GetEnumerator();
        }

        /// <summary>
        /// Initialises an empty category.
        /// </summary>
        public Category()
        {
            _phrases = new Dictionary<string, int>();
        }
        /// <summary>
        /// Initialises a category, populating it with phrases.
        /// </summary>
        /// <param name="phrases">The phrases to add.</param>
        public Category(IEnumerable<string> phrases)
        {
            _phrases = new Dictionary<string, int>();
            foreach (string phrase in phrases)
                Add(phrase);
        }
        /// <summary>
        /// Initialises a category.
        /// </summary>
        /// <param name="phrases">A dictionary containing the category's phrases and their counts.</param>
        public Category(Dictionary<string, int> phrases)
        {
            _phrases = phrases;
        }

        /// <summary>
        /// Gets or sets the count of a certain phrase.
        /// </summary>
        /// <param name="i">The phrase to get or set the count of.</param>
        /// <returns>The count of the given phrase.</returns>
        public int this[string i]
        {
            get { return _phrases.ContainsKey(i) ? _phrases[i] : 0; }
            set { _phrases[i] = value; }
        }

        /// <summary>
        /// Calculates the amount of words in the entire category.
        /// </summary>
        /// <returns>The total amount of words in the category.</returns>
        public int PhraseCount()
        {
            return _phrases.Sum(kvp => kvp.Value);
        }

        /// <summary>
        /// Increments the count of the given phrase, adding it if neccessary.
        /// </summary>
        /// <param name="phrase">The phrase to add.</param>
        public void Add(string phrase)
        {
            if (_phrases.ContainsKey(phrase))
                _phrases[phrase]++;
            else
                _phrases.Add(phrase, 1);
        }

        /// <summary>
        /// Decrements the count of the given phrase, removing it if it is null.
        /// </summary>
        /// <param name="phrase">The phrase to remove.</param>
        public void Remove(string phrase)
        {
            if (!_phrases.ContainsKey(phrase)) return;
            _phrases[phrase]--;
            if (_phrases[phrase] == 0)
                _phrases.Remove(phrase);
        }
    }
}