using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace RxInvestigation
{
    [TestFixture]
    public class KeyTypes
    {
        [Test]
        public void SubjectExample()
        {
            var subject = new Subject<string>();
            var observations = new StringBuilder();

            subject.OnNext("a");
            
            subject.Subscribe(s => observations.Append(s));

            subject.OnNext("b");
            subject.OnNext("c");

            Assert.That(observations.ToString(), Is.EqualTo("bc"));
        }

        [Test]
        public void ReplaySubjectExample()
        {
            var subject = new ReplaySubject<string>(2);
            var observations = new StringBuilder();

            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnNext("c");
            
            subject.Subscribe(s => observations.Append(s));

            subject.OnNext("d");

            Assert.That(observations.ToString(), Is.EqualTo("bcd"));            
        }

        [Test]
        public void ReplaySubjectWindowExample()
        {
            var window = TimeSpan.FromMilliseconds(150);
            var subject = new ReplaySubject<string>(window);
            var observations = new StringBuilder();

            subject.OnNext("a");
            Thread.Sleep(100);
            subject.OnNext("b");
            Thread.Sleep(100);

            subject.Subscribe(s => observations.Append(s));

            subject.OnNext("c");

            Assert.That(observations.ToString(), Is.EqualTo("bc"));
        }

        [Test]
        public void BehaviorSubjectWithNoNext()
        {
            var subject = new BehaviorSubject<string>("a");
            var observations = new StringBuilder();
            
            subject.Subscribe(s => observations.Append(s));

            Assert.That(observations.ToString(), Is.EqualTo("a"));
        }

        [Test]
        public void BehaviorSubjectWithNext()
        {
            var subject = new BehaviorSubject<string>("a");
            var observations = new StringBuilder();
            
            subject.OnNext("b");
            subject.OnNext("c");
            subject.Subscribe(s => observations.Append(s));
            subject.OnNext("d");

            Assert.That(observations.ToString(), Is.EqualTo("cd"));
        }

        [Test]
        public void BehaviorSubjectCompleted()
        {
            var subject = new BehaviorSubject<string>("a");
            var observations = new StringBuilder();

            subject.OnNext("b");
            subject.OnNext("c");
            subject.OnCompleted();
            subject.Subscribe(s => observations.Append(s));
            
            Assert.That(observations.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void AsyncSubjectNotCompleted()
        {
            var subject = new AsyncSubject<string>();
            var observations = new StringBuilder();

            subject.Subscribe(s => observations.Append(s));
            subject.OnNext("a");
            subject.OnNext("b");

            Assert.That(observations.ToString(), Is.Empty);
        }

        [Test]
        public void AsyncSubjectCompleted()
        {
            var subject = new AsyncSubject<string>();
            var observations = new StringBuilder();

            subject.Subscribe(s => observations.Append(s));
            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnCompleted();

            Assert.That(observations.ToString(), Is.EqualTo("b"));
        }

        [Test]
        public void UseFactoryCorrectly()
        {
            var observable = Observable.Create<string>(o =>
                {
                    o.OnNext("a");
                    o.OnNext("b");
                    o.OnNext("c");
                    o.OnCompleted();
                    return Disposable.Empty;
                });
            var observations = new StringBuilder();
            observable.Subscribe(s => observations.Append(s));

            Assert.That(observations.ToString(), Is.EqualTo("abc"));
        }
    }
}