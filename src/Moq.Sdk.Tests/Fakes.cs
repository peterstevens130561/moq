﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Avatars;

namespace Moq.Sdk.Tests
{
    public class FakeMock : IAvatar, IMocked
    {
        private readonly DefaultMock mock;

        protected BehaviorPipeline Pipeline = new BehaviorPipeline();

        public FakeMock() => mock = new DefaultMock(this);

        public IList<IAvatarBehavior> Behaviors => Pipeline.Behaviors;

        public IMock Mock => mock;
    }

    public class FakeSetup : IMockSetup
    {
        public Func<IMethodInvocation, bool> AppliesTo { get; set; } = m => true;

        public FakeInvocation Invocation { get; set; } = new FakeInvocation();

        public IArgumentMatcher[] Matchers { get; set; } = new IArgumentMatcher[0];

        public Times? Occurrence { get; set; }

        public StateBag State { get; } = new StateBag();

        IMethodInvocation IMockSetup.Invocation => Invocation;

        public bool Equals(IMockSetup other) => base.Equals(other);

        bool IMockSetup.AppliesTo(IMethodInvocation actualInvocation) => AppliesTo(actualInvocation);
    }

    public class FakeInvocation : IMethodInvocation
    {
        public FakeInvocation() => Target = new Mocked();
        
        public IArgumentCollection Arguments { get; set; } 

        public IDictionary<string, object> Context { get; set; }

        public MethodBase MethodBase { get; set; }

        public object Target { get; set; }

        public HashSet<Type> SkipBehaviors { get; } = new HashSet<Type>();

        public IMethodReturn CreateExceptionReturn(Exception exception) => new FakeReturn { Exception = exception };

        public IMethodReturn CreateValueReturn(object returnValue, params object[] allArguments) => new FakeReturn { ReturnValue = returnValue };

        public bool Equals(IMethodInvocation other) => base.Equals(other);

        public bool Equals(object other, IEqualityComparer comparer) => base.Equals(other);

        public int GetHashCode(IEqualityComparer comparer) => base.GetHashCode();
    }

    public class FakeReturn : IMethodReturn
    {
        public IDictionary<string, object> Context { get; set; }

        public Exception Exception { get; set; }

        public IArgumentCollection Outputs { get; set; }

        public object ReturnValue { get; set; }
    }
}
