using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DependencyInjectionContainer;
using NUnit.Framework;

namespace DIContainerTests
{
    public struct NotReferenceType
    {
        private int a;
    }

    public static class StaticClass
    {
        private static int f;
    }

    public abstract class AbsractClass
    {
        private static int c;
    }

    public interface ISingleDependency
    {
    }

    public class SingleDependency : ISingleDependency
    {
        private readonly int a = 0;
        private readonly char b = 'a';

        public override bool Equals(object obj)
        {
            if (obj is SingleDependency sd)
                return sd.a == a && sd.b == b;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + b.GetHashCode();
            return hash;
        }
    }

    public class ClassWithInnerDependency : ISingleDependency
    {
        private readonly int a = 0;
        private readonly IInnerDependency d;

        public ClassWithInnerDependency(IInnerDependency dependency)
        {
            d = dependency;
        }

        public override bool Equals(object obj)
        {
            if (obj is ClassWithInnerDependency id)
                return id.a == a && id.d.Equals(d);
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + d.GetHashCode();
            return hash;
        }
    }

    public interface IInnerDependency
    {
    }

    public class InnerDependencyClass : IInnerDependency
    {
        private readonly int a = 5;
        private readonly int b = 2;

        public override bool Equals(object obj)
        {
            if (obj is InnerDependencyClass idc)
                return idc.a == a && idc.b == b;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + b.GetHashCode();
            return hash;
        }
    }

    public interface ISingleton
    {
    }

    public class SingletonClass : ISingleton
    {
        public SingletonClass(int value)
        {
            a = value;
        }

        public int a { get; }
    }

    public interface ICollectionClass
    {
    }

    public class CollectionClass1 : ICollectionClass
    {
        private readonly int a = 0;
        private readonly char b = 'a';

        public override bool Equals(object obj)
        {
            if (obj is CollectionClass1 c)
                return c.a == a && c.b == b;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + b.GetHashCode();
            return hash;
        }
    }

    public class CollectionClass2 : ICollectionClass
    {
        private readonly char a = 'b';
        private readonly int b = 1;

        public override bool Equals(object obj)
        {
            if (obj is CollectionClass2 c)
                return c.a == a && c.b == b;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + b.GetHashCode();
            return hash;
        }
    }

    public class CollectionClass3 : ICollectionClass
    {
        private readonly int a = 3;
        private readonly int b = 4;

        public override bool Equals(object obj)
        {
            if (obj is CollectionClass3 c)
                return c.a == a && c.b == b;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + b.GetHashCode();
            return hash;
        }
    }

    public class CollectionClass4 : ICollectionClass
    {
        private readonly char a = 'z';
        private readonly char b = 'x';

        public override bool Equals(object obj)
        {
            if (obj is CollectionClass4 c)
                return c.a == a && c.b == b;
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + a.GetHashCode();
            hash = hash * 23 + b.GetHashCode();
            return hash;
        }
    }

    public interface ICollectionCtor
    {
    }

    public class CollectionCtor : ICollectionCtor
    {
        private readonly IEnumerable<ICollectionClass> collection;

        public CollectionCtor(IEnumerable<ICollectionClass> collection)
        {
            this.collection = collection;
        }

        public override bool Equals(object obj)
        {
            if (obj is CollectionCtor c) return c.collection.SequenceEqual(collection);
            return false;
        }

        public override int GetHashCode()
        {
            return collection.GetHashCode();
        }
    }


    public interface IConstrained
    {
    }

    public class ConstrainedClass : IConstrained
    {
        private readonly int a = 5;

        public override bool Equals(object obj)
        {
            if (obj is ConstrainedClass c) return c.a == a;
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode();
        }
    }

    public class ConstrainedClass2 : IConstrained
    {
        private readonly int a = 3;

        public override bool Equals(object obj)
        {
            if (obj is ConstrainedClass2 c) return c.a == a;
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode();
        }
    }

    public interface IGenericConstrained<TConstrainedClass> where TConstrainedClass : IConstrained
    {
    }

    internal class GenericConstrainedClass<TConstrainedClass> : IGenericConstrained<TConstrainedClass>
        where TConstrainedClass : IConstrained
    {
        private TConstrainedClass cls;

        public GenericConstrainedClass(TConstrainedClass @class)
        {
            cls = @class;
        }

        public override bool Equals(object obj)
        {
            if (obj is GenericConstrainedClass<TConstrainedClass> c) return c.cls.Equals(cls);
            return false;
        }

        public override int GetHashCode()
        {
            return cls.GetHashCode();
        }
    }

    public interface IOpenConstrained
    {
    }

    public class OpenConstrainedClass : IOpenConstrained
    {
        private readonly int a = 5;

        public override bool Equals(object obj)
        {
            if (obj is OpenConstrainedClass c) return c.a == a;
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode();
        }
    }

    public class OpenConstrainedClass2 : IOpenConstrained
    {
        private readonly int a = 3;

        public override bool Equals(object obj)
        {
            if (obj is OpenConstrainedClass2 c) return c.a == a;
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode();
        }
    }

    public interface IOpenGenericConstrained<TOpenConstrainedClass> where TOpenConstrainedClass : IOpenConstrained
    {
    }

    internal class OpenGenericConstrainedClass<TOpenConstrainedClass> : IOpenGenericConstrained<TOpenConstrainedClass>
        where TOpenConstrainedClass : IOpenConstrained
    {
        private TOpenConstrainedClass cls;

        public OpenGenericConstrainedClass(TOpenConstrainedClass @class)
        {
            cls = @class;
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenGenericConstrainedClass<TOpenConstrainedClass> c) return c.cls.Equals(cls);
            return false;
        }

        public override int GetHashCode()
        {
            return cls.GetHashCode();
        }
    }

    public enum NamedDependency
    {
        First,
        Second,
        Third
    }

    public class AnotherSimpleClass : ISingleDependency
    {
        private readonly ISingleDependency dependency;

        public AnotherSimpleClass([DependencyKey(NamedDependency.Second)]
            ISingleDependency dep)
        {
            dependency = dep;
        }

        public override bool Equals(object obj)
        {
            if (obj is AnotherSimpleClass c) return c.dependency.Equals(dependency);
            return false;
        }

        public override int GetHashCode()
        {
            return dependency.GetHashCode();
        }
    }

    public class Tests
    {
        private DependenciesConfigurator dependencies;
        private DependencyProvider provider;

        [SetUp]
        public void Setup()
        {
            dependencies = new DependenciesConfigurator();

            dependencies.Register<ISingleDependency, SingleDependency>();
            dependencies.Register<ISingleDependency, ClassWithInnerDependency>();
            dependencies.Register<IInnerDependency, InnerDependencyClass>();
            dependencies.Register<ISingleton, SingletonClass>(DependenciesConfigurator.Lifetime.Singleton);
            dependencies.Register<ICollectionClass, CollectionClass1>();
            dependencies.Register<ICollectionClass, CollectionClass2>();
            dependencies.Register<ICollectionClass, CollectionClass3>();
            dependencies.Register<ICollectionClass, CollectionClass4>();
            dependencies.Register<ICollectionCtor, CollectionCtor>();
            dependencies.Register<IConstrained, ConstrainedClass>();
            dependencies.Register<IConstrained, ConstrainedClass2>();
            dependencies.Register<IGenericConstrained<IConstrained>, GenericConstrainedClass<IConstrained>>();
            dependencies.Register<IOpenConstrained, OpenConstrainedClass>();
            dependencies.Register<IOpenConstrained, OpenConstrainedClass2>();
            dependencies.Register(typeof(IOpenGenericConstrained<>), typeof(OpenGenericConstrainedClass<>));
            dependencies.Register<ISingleDependency, AnotherSimpleClass>();
            provider = new DependencyProvider(dependencies);
        }

        [Test]
        public void SimpleDependencyTest()
        {
            var actual = provider.Resolve<ISingleDependency>();
            var expected = new SingleDependency();

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ClassWithInnerDependencyTest()
        {
            var actual = provider.Resolve<ISingleDependency>(NamedDependency.Second);
            var expected = new ClassWithInnerDependency(new InnerDependencyClass());

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SingletonTest()
        {
            var actual = Task.Run(() => provider.Resolve<ISingleton>());
            var expected1 = Task.Run(() => provider.Resolve<ISingleton>());
            var expected2 = Task.Run(() => provider.Resolve<ISingleton>());
            var expected3 = Task.Run(() => provider.Resolve<ISingleton>());
            var expected4 = Task.Run(() => provider.Resolve<ISingleton>());
            var expected5 = Task.Run(() => provider.Resolve<ISingleton>());

            Assert.AreEqual(expected1.Result, actual.Result);
            Assert.AreEqual(expected2.Result, actual.Result);
            Assert.AreEqual(expected3.Result, actual.Result);
            Assert.AreEqual(expected4.Result, actual.Result);
            Assert.AreEqual(expected5.Result, actual.Result);
        }

        [Test]
        public void CollectionTest()
        {
            var actual = provider.Resolve<IEnumerable<ICollectionClass>>();
            var expected = new ICollectionClass[]
                {new CollectionClass1(), new CollectionClass2(), new CollectionClass3(), new CollectionClass4()};

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void CollectionCtorTest()
        {
            var actual = provider.Resolve<ICollectionCtor>();
            var expected = new CollectionCtor(new ICollectionClass[]
                {new CollectionClass1(), new CollectionClass2(), new CollectionClass3(), new CollectionClass4()});

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConstrainedClassTest()
        {
            var actual = provider.Resolve<IGenericConstrained<IConstrained>>();
            var expected = new GenericConstrainedClass<IConstrained>(new ConstrainedClass());

            var actual2 = provider.Resolve<IGenericConstrained<IConstrained>>(NamedDependency.Second);
            var expected2 = new GenericConstrainedClass<IConstrained>(new ConstrainedClass2());

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected2, actual2);
            Assert.AreNotEqual(expected2, actual);
        }

        [Test]
        public void OpenConstrainedClassTest()
        {
            var actual = provider.Resolve<IOpenGenericConstrained<IOpenConstrained>>();
            var expected = new OpenGenericConstrainedClass<IOpenConstrained>(new OpenConstrainedClass());

            var actual2 = provider.Resolve<IOpenGenericConstrained<IOpenConstrained>>(NamedDependency.Second);
            var expected2 = new OpenGenericConstrainedClass<IOpenConstrained>(new OpenConstrainedClass2());

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected2, actual2);
            Assert.AreNotEqual(expected2, actual);
        }

        [Test]
        public void DependencyKeyTest()
        {
            var actual = provider.Resolve<ISingleDependency>(NamedDependency.Third);
            var expected = new AnotherSimpleClass(new ClassWithInnerDependency(new InnerDependencyClass()));

            Assert.AreEqual(expected, actual);
        }
    }
}