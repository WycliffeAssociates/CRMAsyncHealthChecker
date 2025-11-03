using System;
using System.Collections.Generic;
using CRMAsyncHealthChecker;
using FakeXrmEasy;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace CRMAsynHealthCheckerTests
{
    [TestClass]
    public class Tests
    {
        private IXrmFakedContext context;
        private IOrganizationService service;
        private List<Entity> initialEntities;

        [TestInitialize]
        public void SetUp()
        {
            this.context = MiddlewareBuilder.New()
                .SetLicense(FakeXrmEasyLicense.NonCommercial)
                .AddCrud()
                .UseCrud()
                .Build();
            this.initialEntities = new List<Entity>();
        }

        [TestMethod]
        public void TestUnderLimit()
        {
            Entity recordOne = new Entity("asyncoperation", Guid.NewGuid());
            recordOne["statuscode"] = new OptionSetValue(0);
            this.initialEntities.Add(recordOne);
            this.context.Initialize(this.initialEntities);
            this.service = this.context.GetOrganizationService();

            Assert.IsFalse(Program.CheckRecordsPastLimit(this.service, 2));
        }

        [TestMethod]
        public void TestOverLimit()
        {
            Entity recordOne = new Entity("asyncoperation", Guid.NewGuid());
            recordOne["statuscode"] = new OptionSetValue(0);
            Entity recordTwo = new Entity("asyncoperation", Guid.NewGuid());
            recordTwo["statuscode"] = new OptionSetValue(0);
            Entity recordThree = new Entity("asyncoperation", Guid.NewGuid());
            recordThree["statuscode"] = new OptionSetValue(0);
            this.initialEntities.Add(recordOne);
            this.initialEntities.Add(recordTwo);
            this.initialEntities.Add(recordThree);
            this.context.Initialize(this.initialEntities);
            this.service = this.context.GetOrganizationService();

            Assert.IsTrue(Program.CheckRecordsPastLimit(this.service, 2));
        }
    }
}
