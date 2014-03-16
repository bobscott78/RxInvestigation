using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using NUnit.Framework;

namespace RxInvestigation
{
    [TestFixture]
    class SequenceBasics
    {
        [Test]
        public void Generators()
        {
            //var range = Observable.Range(10, 15);
            var range = Observable.Generate(10, value => value < 25, value => value + 1, value => value);
            var observations = new List<int>();
            range.Subscribe(observations.Add);

            Assert.That(observations.Count, Is.EqualTo(15));
            Assert.That(observations[14], Is.EqualTo(24));
        }

        [Test]
        public void TimerExample()
        {
            var timer = Observable.Timer(TimeSpan.FromMilliseconds(250));
            timer.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        [Test]
        public void StartExample()
        {
            var start = Observable.Start(() =>
                {
                    Console.WriteLine("Working away");
                    for (var i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        Console.WriteLine(i);
                    }
                    return "published value";
                });
            start.Subscribe(Console.WriteLine, () => Console.WriteLine("Action completed"));

            Thread.Sleep(1100);
        }
    }
}
