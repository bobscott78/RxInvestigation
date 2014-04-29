using System;
using System.Reactive.Linq;
using System.Text;
using NUnit.Framework;

namespace RxInvestigation
{
    public class ReducingASequence
    {
        [Test]
        public void WhereExample()
        {
            var observations = new StringBuilder();
            Observable.Range(0, 10)
                      .Where(o => o%2 == 0)
                      .Subscribe(s => observations.Append(s));

            Assert.That(observations.ToString(), Is.EqualTo("02468"));
        }
    }
}