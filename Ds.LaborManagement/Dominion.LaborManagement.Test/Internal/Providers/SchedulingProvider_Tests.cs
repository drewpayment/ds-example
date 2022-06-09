using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Services.Api;
using Dominion.Core.Test.Helpers.TestObjects.Mocks;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.LaborManagement.Test.Helpers.Mocks;
using Dominion.Taxes.Test.Helpers.Mocks;
using Dominion.Utility.DataExport.Exporters;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpTasks;
using Dominion.Utility.Query.LinqKit;
using Moq;
using NUnit.Framework;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.Containers;

namespace Dominion.LaborManagement.Test.Internal.Providers
{
    [TestFixture]
    public class SchedulingProvider_Tests
    {
        private class SchedulingProviderMocks : ApiSessionMockBase
        {
            #region Properties and Variables

            private readonly ISchedulingProvider _provider;
            public ClockEmployeeCostCenterQueryMockConfig ClockEmployeeCostCenterQueryMockConfig { get; set; }
            public EmployeeSchedulePreviewQueryMockConfig EmployeeSchedulePreviewQueryMockConfig { get; set; }
            public ClockEmployeeScheduleQueryMockConfig ClockEmployeeScheduleQueryMockConfig { get; set; }
            public EmployeeDefaultShiftQueryMockConfig EmployeeDefaultShiftQueryMockConfig { get; set; }

            public ISchedulingProvider Provider
            {
                get
                {
                    //_provider = _provider ?? new LeaveManagementProvider(BusinessApiSessionMoq.Object);
                    return _provider;
                }
            }

            #endregion

            #region Constructors and Initializers

            public SchedulingProviderMocks()
            {
                 var groupProvider = new ScheduleGroupProvider(BusinessApiSessionMoq.Object) as IScheduleGroupProvider;
                _provider = new SchedulingProvider(BusinessApiSessionMoq.Object, groupProvider);
                
                this.ClockEmployeeCostCenterQueryMockConfig = new ClockEmployeeCostCenterQueryMockConfig();
                this.EmployeeSchedulePreviewQueryMockConfig = new EmployeeSchedulePreviewQueryMockConfig();
                this.ClockEmployeeScheduleQueryMockConfig = new ClockEmployeeScheduleQueryMockConfig();
                this.EmployeeDefaultShiftQueryMockConfig = new EmployeeDefaultShiftQueryMockConfig();

                this.LaborManagementRepositoryMoq
                    .Setup(x => x.ClockEmployeeCostCenterQuery())
                    .Returns(this.ClockEmployeeCostCenterQueryMockConfig.Mock.Object);

                this.LaborManagementRepositoryMoq
                    .Setup(x => x.EmployeeSchedulePreviewQuery())
                    .Returns(this.EmployeeSchedulePreviewQueryMockConfig.Mock.Object);
                
                this.LaborManagementRepositoryMoq
                    .Setup(x => x.ClockEmployeeScheduleQuery())
                    .Returns(this.ClockEmployeeScheduleQueryMockConfig.Mock.Object);

                this.LaborManagementRepositoryMoq
                    .Setup(x => x.EmployeeDefaultShiftQuery())
                    .Returns(this.EmployeeDefaultShiftQueryMockConfig.Mock.Object);

            }

            #endregion

            public EmployeeSchedulesPersistDto SaveData
            {
                get
                {
                    var data = new EmployeeSchedulesPersistDto()
                    {
                        ScheduleSourceId = int.MinValue,
                        EmployeeSchedulesDtos = new List<EmployeeSchedulesDto>()
                        {
                            new EmployeeSchedulesDto()
                            {
                                EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                                {
                                    new ScheduleShiftDto(),
                                    new ScheduleShiftDto()
                                    {
                                        IsPreview = true,
                                    },
                                },
                                EmployeeRecurringShifts = new List<ScheduleShiftDto>()
                                {
                                    new ScheduleShiftDto(),
                                },
                            },
                        }
                    };

                    return data;
                }
            }
        }

        #region ISchedulingProvider Methods

        [Test]
        public void GetEmployeeScheduleShifts()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();

            mocks.OpTasksFactoryMoq.SetExecuteAction<
                SchedulingProvider.FixStartEndTimeForPublished,
                IEnumerable<EmployeeSchedulesDto>>(1);

            mocks.OpTasksFactoryMoq.SetExecuteFunc<
                SchedulingProvider.FlattenScheduleData,
                IEnumerable<ScheduleShiftDto>,
                IEnumerable<EmployeeSchedulesDto>>(2);

            mocks.OpTasksFactoryMoq.SetExecuteAction<
                SchedulingProvider.ProcessGetShiftData,
                List<EmployeeSchedulesDto>,
                IEnumerable<ScheduleShiftDto>,
                IEnumerable<EmployeeSchedulesDto>>();

            //ACT
            var results = mocks.Provider.GetEmployeeScheduleShifts(
                clientId: 100, 
                startDate: DateTime.Now,
                endDate: DateTime.Now,
                scheduleGroupId: int.MinValue,
                scheduleGroupSourceId: int.MinValue);

            //ASSERT

            mocks.OpTasksFactoryMoq.VerifyAll();

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteFunc(
                It.IsAny<SchedulingProvider.SplitPublishedRawData>(),
                It.IsAny<IEnumerable<ClockEmployeeScheduleShiftDto>>(),
                It.IsAny<int?>(),
                It.IsAny<IScheduleGroupProvider>()),
                Times.Exactly(2));

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteFunc(
                It.IsAny<SchedulingProvider.SplitPublishedRawData>(),
                It.IsAny<IEnumerable<ClockEmployeeScheduleShiftDto>>(),
                It.Is<int?>(y => y.HasValue),
                It.IsAny<IScheduleGroupProvider>()),
                Times.Exactly(1));

            //---------------------------------------
            //ClockEmployeeCostCenterQuery
            //---------------------------------------
            mocks.ClockEmployeeCostCenterQueryMockConfig.Mock
                .Verify(x => x.ByCostCenterId(
                    It.IsAny<int>()),
                    Times.Once());

            mocks.ClockEmployeeCostCenterQueryMockConfig.Mock
                .Verify(x => x.ExecuteQueryAs(
                    It.IsAny<SchedulingMaps.ToEmployeesToScheduleForCostCenter>()),
                    Times.Once());

            //---------------------------------------
            //EmployeeSchedulePreviewQuery
            //---------------------------------------
            mocks.EmployeeSchedulePreviewQueryMockConfig.Mock
                .Verify(x => x.ByScheduleGroupId(
                    It.IsAny<int>()),
                    Times.Once);

            mocks.EmployeeSchedulePreviewQueryMockConfig.Mock
                .Verify(x => x.ByEventDateRange(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()),
                    Times.Exactly(2));

            mocks.EmployeeSchedulePreviewQueryMockConfig.Mock
                .Verify(x => x.ByScheduleGroupsOtherThanId(
                    It.IsAny<int>()),
                    Times.Once());

            mocks.EmployeeSchedulePreviewQueryMockConfig.Mock
                .Verify(x => x.ExecuteQueryAs(
                    It.IsAny<SchedulingMaps.ToPreviewSchedule>()),
                    Times.Exactly(2));

            //---------------------------------------
            //ClockEmployeeScheduleQuery
            //---------------------------------------
            mocks.ClockEmployeeScheduleQueryMockConfig.Mock
                .Verify(x => x.ByEventDateRange(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()),
                    Times.Exactly(2));

            mocks.ClockEmployeeScheduleQueryMockConfig.Mock
                .Verify(x => x.ByScheduleGroupId(
                    It.IsAny<int>()),
                    Times.Once());

            mocks.ClockEmployeeScheduleQueryMockConfig.Mock
                .Verify(x => x.ByScheduleGroupsOtherThanId(
                    It.IsAny<int>()),
                    Times.Once());

            mocks.ClockEmployeeScheduleQueryMockConfig.Mock
                .Verify(x => x.ByEmployeeIds(
                    It.IsAny<IEnumerable<int>>()),
                    Times.Once());

            mocks.ClockEmployeeScheduleQueryMockConfig.Mock
                .Verify(x => x.ExecuteQueryAs(
                    It.IsAny<SchedulingMaps.ToClockEmployeeSchedule>()),
                    Times.Exactly(2));

            //---------------------------------------
            //EmployeeDefaultShiftQuery
            //---------------------------------------
            mocks.EmployeeDefaultShiftQueryMockConfig.Mock
                .Verify(x => x.ByEmployeesIds(
                    It.IsAny<IEnumerable<int>>()),
                    Times.Once());

            mocks.EmployeeDefaultShiftQueryMockConfig.Mock
                .Verify(x => x.ByScheduleGroupId(
                    It.IsAny<int>()),
                    Times.Once());

            mocks.EmployeeDefaultShiftQueryMockConfig.Mock
                .Verify(x => x.ExecuteQueryAs(
                    It.IsAny<SchedulingMaps.ToEmployeeDefaultShift>()),
                    Times.Once());

            //var previewShifts = _session.OpTasksFactory.ExecuteFunc(
            //    new FlattenScheduleData(), 
            //    preview);

        }

        [Test]
        public void SaveOrUpdateEmployeeScheduleShifts_AllDataPresent()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();

            //ACT
            var results = mocks.Provider.SaveOrUpdateEmployeeScheduleShifts(
                mocks.SaveData);

            //ASSERT
            mocks.UnitOfWorkMoq.Verify(x => x.Commit(), Times.Once);

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteAction(
                It.IsAny<SchedulingProvider.SavePreview>(),
                It.IsAny<IEnumerable<ScheduleShiftDto>>()),
                Times.Once);

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteAction(
                It.IsAny<SchedulingProvider.SavePublished>(),
                It.IsAny<IEnumerable<ScheduleShiftDto>>(),
                It.IsAny<int>()),
                Times.Once);

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteAction(
                It.IsAny<SchedulingProvider.SaveRecurringShifts>(),
                It.IsAny<IEnumerable<ScheduleShiftDto>>()),
                Times.Once);
        }

        [Test]
        public void SaveOrUpdateEmployeeScheduleShifts_NoDataPresent()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var data = mocks.SaveData;
            var obj = data.EmployeeSchedulesDtos.First();
            obj.EmployeeScheduleShifts.Clear();
            obj.EmployeeRecurringShifts.Clear();

            //ACT
            var results = mocks.Provider.SaveOrUpdateEmployeeScheduleShifts(
                data);

            //ASSERT
            mocks.UnitOfWorkMoq.Verify(x => x.Commit(), Times.Once);

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteAction(
                It.IsAny<SchedulingProvider.SavePreview>(),
                It.IsAny<IEnumerable<ScheduleShiftDto>>()),
                Times.Never);

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteAction(
                It.IsAny<SchedulingProvider.SavePublished>(),
                It.IsAny<IEnumerable<ScheduleShiftDto>>(),
                It.IsAny<int>()),
                Times.Never);

            mocks.OpTasksFactoryMoq.Mock.Verify(x => x.ExecuteAction(
                It.IsAny<SchedulingProvider.SaveRecurringShifts>(),
                It.IsAny<IEnumerable<ScheduleShiftDto>>()),
                Times.Never);
        }

        #endregion

        #region Provider Actions and Functions (Saving Only)

        #region Save Preview

        [Test]
        public void SavePreview_Delete()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var obj = new SchedulingProvider.SavePreview(mocks.BusinessApiSessionMoq.Object);
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    IsDeleted = true,
                    ShiftId = int.MaxValue
                },
            };

            //ACT
            obj.Execute(shifts);

            mocks.BusinessApiSessionMoq.Verify(x =>
                x.SetModifiedProperties(It.IsAny<IHasModifiedData>(), null),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<EmployeeSchedulePreview>()),
                Times.Once);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterNew(It.IsAny<EmployeeSchedulePreview>()),
                Times.Never);
        }

        [Test]
        public void SavePreview_Add()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var obj = new SchedulingProvider.SavePreview(mocks.BusinessApiSessionMoq.Object);
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    IsAdded = true,
                    ShiftId = int.MaxValue
                },
            };

            //ACT
            obj.Execute(shifts);

            mocks.BusinessApiSessionMoq.Verify(x =>
                x.SetModifiedProperties(It.IsAny<IHasModifiedData>(), null),
                Times.Once);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<EmployeeSchedulePreview>()),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterNew(It.IsAny<EmployeeSchedulePreview>()),
                Times.Once);
        }

        #endregion

        #region Save Published

        [Test]
        public void SavePublished_Add()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var normalizeShiftDateTimesMock = new Mock<IAdHocAction<ScheduleShiftDto>>();
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                    IsAdded = true,
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                    IsAdded = true,
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                    IsAdded = true,
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 2,
                    EventDate = new DateTime(1900, 1, 1),
                    IsAdded = true,
                },
            };

            var obj = new SchedulingProvider.SavePublished(
                mocks.BusinessApiSessionMoq.Object,
                normalizeShiftDateTimesMock.Object);

            //ACT
            obj.Execute(shifts, int.MinValue);

            //ASSERT
            normalizeShiftDateTimesMock.Verify(x => //3 times for emp1, 1 time for emp2
                x.Execute(It.IsAny<ScheduleShiftDto>()),
                Times.Exactly(4));

            mocks.BusinessApiSessionMoq.Verify(x => //1 times for emp1, 1 time for emp2
                x.SetModifiedProperties(It.IsAny<IHasModifiedOptionalData>(), null),
                Times.Exactly(2));

            mocks.UnitOfWorkMoq.Verify(x => //1 times for emp1, 1 time for emp2
                x.RegisterNew(It.IsAny<ClockEmployeeSchedule>()),
                Times.Exactly(2));

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<ClockEmployeeSchedule>()),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterModified(
                    It.IsAny<ClockEmployeeSchedule>(),
                    default(PropertyList<ClockEmployeeSchedule>)),
                Times.Never);
        }

        [Test]
        public void SavePublished_Delete()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var normalizeShiftDateTimesMock = new Mock<IAdHocAction<ScheduleShiftDto>>();
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                    IsDeleted = true,
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 2,
                    EventDate = new DateTime(1900, 1, 1),
                    IsDeleted = true,
                },
            };

            var obj = new SchedulingProvider.SavePublished(
                mocks.BusinessApiSessionMoq.Object,
                normalizeShiftDateTimesMock.Object);

            //ACT
            obj.Execute(shifts, int.MinValue);

            //ASSERT
            normalizeShiftDateTimesMock.Verify(x => //3 times for emp1, 1 time for emp2
                x.Execute(It.IsAny<ScheduleShiftDto>()),
                Times.Never);

            mocks.BusinessApiSessionMoq.Verify(x => //1 times for emp1, 1 time for emp2
                x.SetModifiedProperties(It.IsAny<IHasModifiedOptionalData>(), null),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x => //1 times for emp1, 1 time for emp2
                x.RegisterNew(It.IsAny<ClockEmployeeSchedule>()),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<ClockEmployeeSchedule>()),
                Times.Exactly(2));

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterModified(
                    It.IsAny<ClockEmployeeSchedule>(),
                    default(PropertyList<ClockEmployeeSchedule>)),
                Times.Never);
        }

        [Test]
        public void SavePublished_Update()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var normalizeShiftDateTimesMock = new Mock<IAdHocAction<ScheduleShiftDto>>();
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                    IsDeleted = true,
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EventDate = new DateTime(1900, 1, 1),
                },
            };

            var obj = new SchedulingProvider.SavePublished(
                mocks.BusinessApiSessionMoq.Object,
                normalizeShiftDateTimesMock.Object);

            //ACT
            obj.Execute(shifts, int.MinValue);

            //ASSERT
            normalizeShiftDateTimesMock.Verify(x =>
                x.Execute(It.IsAny<ScheduleShiftDto>()),
                Times.Exactly(2));

            mocks.BusinessApiSessionMoq.Verify(x =>
                x.SetModifiedProperties(It.IsAny<IHasModifiedOptionalData>(), null),
                Times.Once);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterNew(It.IsAny<ClockEmployeeSchedule>()),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<ClockEmployeeSchedule>()),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterModified(
                    It.IsAny<ClockEmployeeSchedule>(),
                    default(PropertyList<ClockEmployeeSchedule>)),
                Times.Once);
        }

        #endregion

        #region Save Preview

        [Test]
        public void SaveRecurringShifts_Delete()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var obj = new SchedulingProvider.SaveRecurringShifts(mocks.BusinessApiSessionMoq.Object);
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    IsDeleted = true,
                    ShiftId = int.MaxValue
                },
            };

            //ACT
            obj.Execute(shifts);

            mocks.BusinessApiSessionMoq.Verify(x =>
                x.SetModifiedProperties(It.IsAny<IHasModifiedData>(), null),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<EmployeeDefaultShift>()),
                Times.Once);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterNew(It.IsAny<EmployeeDefaultShift>()),
                Times.Never);
        }

        [Test]
        public void SaveRecurringShifts_Add()
        {
            //ARRANGE
            var mocks = new SchedulingProviderMocks();
            var obj = new SchedulingProvider.SaveRecurringShifts(mocks.BusinessApiSessionMoq.Object);
            var shifts = new List<ScheduleShiftDto>()
            {
                new ScheduleShiftDto()
                {
                    IsAdded = true,
                    ShiftId = int.MaxValue
                },
            };

            //ACT
            obj.Execute(shifts);

            mocks.BusinessApiSessionMoq.Verify(x =>
                x.SetModifiedProperties(It.IsAny<IHasModifiedData>(), null),
                Times.Once);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterDeleted(It.IsAny<EmployeeDefaultShift>()),
                Times.Never);

            mocks.UnitOfWorkMoq.Verify(x =>
                x.RegisterNew(It.IsAny<EmployeeDefaultShift>()),
                Times.Once);
        }

        #endregion

        #endregion

        #region Provider Actions and Functions (Get Only)

        [Test]
        public void FixStartEndTimeForPublished()
        {
            //ARRANGE
            var start = new DateTime(1900, 1, 1, 8, 0, 0);
            var end = new DateTime(1900, 1, 1, 15, 0, 0);
            var obj = new SchedulingProvider.FixStartEndTimeForPublished();
            var data = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto()
                        {
                            IsPreview = false,
                            StartTimeDate = start,
                            EndTimeDate = end,
                        }
                    }
                }
            };

            //ACT
            obj.Execute(data);

            //ASSERT
            Assert.AreEqual(
                start.ToTimeSpan(),
                data.First().EmployeeScheduleShifts.First().StartTime.Value);

            Assert.AreEqual(
                end.ToTimeSpan(),
                data.First().EmployeeScheduleShifts.First().EndTime.Value);
        }

        [Test]
        public void NormalizeShiftDateTimes()
        {
            //ARRANGE
            var start = new DateTime(1900, 1, 1, 8, 0, 0).ToTimeSpan();
            var end = new DateTime(1900, 1, 1, 2, 0, 0).ToTimeSpan();
            var data = new ScheduleShiftDto()
            {
                EventDate = start.ToDateTime().Date,
                StartTime = start,
                EndTime = end,
            };

            //ACT
            new SchedulingProvider.NormalizeShiftDateTimes().Execute(data);

            //ASSERT
            Assert.AreEqual(
                start.ToDateTime(),
                data.StartTimeDate.Value);

            Assert.AreEqual(
                end.ToDateTime().AddDays(1),
                data.EndTimeDate.Value);

        }

        [Test]
        public void CombineAllShiftData()
        {
            //ARRANGE
            var start = new DateTime(1900, 1, 1, 8, 0, 0);
            var end = new DateTime(1900, 1, 1, 15, 0, 0);
            var published = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 2}
                    }
                },
            };

            var preview = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 2}
                    }
                },
            };

            var empCC = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 3,
                },
            };

            var expectedItems = published.ToList()
                .AddRangeEx(published)
                .AddRangeEx(empCC).ToList();

            var empIds = expectedItems.Select(x => x.EmployeeId).Distinct();

            var expectedShifts = expectedItems
                .Where(i => i.EmployeeScheduleShifts != null)
                .SelectMany(i => i.EmployeeScheduleShifts)
                .OrderBy(x => x.GroupScheduleShiftId)
                .ToList();

            //ACT
            var results = new SchedulingProvider.CombineAllShiftData().Execute(
                preview: preview,
                published: published,
                empCC: empCC);

            //ASSERT
            Assert.IsTrue(results.Any(x => empIds.Contains(x.EmployeeId)), "Not all employees were included");
            Assert.IsTrue(results.Count() == empIds.Count(), "Should be one item for each employee");

            results.ForEach(x => Assert.IsTrue(
                x.EmployeeScheduleShifts.Count() == expectedShifts.Count(y => y.EmployeeId == x.EmployeeId),
                "Shift count was off for employee: "+x.EmployeeId
                ));
        }

        [Test]
        public void CombineAllOtherGroupShifts()
        {
            //ARRANGE
            var start = new DateTime(1900, 1, 1, 8, 0, 0);
            var end = new DateTime(1900, 1, 1, 15, 0, 0);
            var data = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1, ScheduleGroupId = 1}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 2, ScheduleGroupId = 1}
                    }
                },
            };

            var otherPreviewData = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1, ScheduleGroupId = 1}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 2, ScheduleGroupId = 2}
                    }
                },
            };


            var otherPublishedData = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1, ScheduleGroupId = 2}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 2, ScheduleGroupId = 1}
                    }
                },
            };

            var expectedItems = data.ToList()
                .AddRangeEx(otherPreviewData)
                .AddRangeEx(otherPublishedData);

            var empIds = expectedItems.Select(x => x.EmployeeId).Distinct();

            var expectedShifts = expectedItems
                .Where(i => i.EmployeeScheduleShifts != null)
                .SelectMany(i => i.EmployeeScheduleShifts.Where(x => x.ScheduleGroupId == 1))
                .ToList();

            //ACT
            new SchedulingProvider.CombineAllOtherGroupShifts().Execute(
                originalScheduleGroupId: 1,
                data: data,
                otherPreviewData: otherPreviewData,
                otherPublishData: otherPublishedData);

            //ASSERT
            Assert.IsTrue(data.Any(x => empIds.Contains(x.EmployeeId)), "Not all employees were included");
            Assert.IsTrue(data.Count() == empIds.Count(), "Should be one item for each employee");

            data.ForEach(x => Assert.IsTrue(
                x.EmployeeScheduleShifts.Count() == expectedShifts.Count(y => y.EmployeeId == x.EmployeeId),
                "Shift count was off for employee: "+x.EmployeeId
                ));
        }

        [Test]
        public void ExtractEmployeeIds()
        {
            //ARRANGE
            var data = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    IsTerminated = false,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    IsTerminated = false,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 2}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 3,
                    IsTerminated = true,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>(),
                },
            };

            //ACT
            var result = new SchedulingProvider.ExtractEmployeeIds().Execute(data);

            //ASSERT
            Assert.IsFalse(result.Any(x => x == 3));
            CollectionAssert.AreEquivalent(new[] {1, 2}, result);
        }

        [Test]
        public void SplitPublishedRawData1()
        {
            //ARRANGE
            var mocks = new ApiSessionMockBase();
            var start = new DateTime(1900, 1, 1, 8, 0, 0);
            var end = new DateTime(1900, 1, 1, 15, 0, 0);
            var scheduleGroupId = 1;

            var data = new List<ClockEmployeeScheduleShiftDto>()
            {
                new ClockEmployeeScheduleShiftDto()
                {
                    EmployeeId = 1,
                    //shift 1
                    ScheduleGroupId1 = scheduleGroupId,
                    StartTimeDate1 = start,
                    EndTimeDate1 = end,
                    //shift 2
                    ScheduleGroupId2 = scheduleGroupId+1,
                    StartTimeDate2 = start,
                    EndTimeDate2 = end,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN1",
                        LastName = "LN1",
                    },
                }
            };

            //ACT
            var groupProvider = new ScheduleGroupProvider(mocks.BusinessApiSessionMoq.Object) as IScheduleGroupProvider;
            var results = new SchedulingProvider.SplitPublishedRawData()
                .Execute(data, scheduleGroupId, groupProvider);

            //ASSERT
            Assert.IsNotNull(results);
            CollectionAssert.AllItemsAreInstancesOfType(results, typeof(EmployeeSchedulesDto));
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.First().EmployeeScheduleShifts.Count() == 1);
        }

        [Test]
        public void SplitPublishedRawData2()
        {
            //ARRANGE
            var mocks = new ApiSessionMockBase();
            var start = new DateTime(1900, 1, 1, 8, 0, 0);
            var end = new DateTime(1900, 1, 1, 15, 0, 0);
            var scheduleGroupId = 1;

            var data = new List<ClockEmployeeScheduleShiftDto>()
            {
                new ClockEmployeeScheduleShiftDto()
                {
                    EmployeeId = 1,
                    //shift 1
                    ScheduleGroupId1 = scheduleGroupId,
                    StartTimeDate1 = start,
                    EndTimeDate1 = end,
                    //shift 2
                    ScheduleGroupId2 = scheduleGroupId,
                    StartTimeDate2 = start,
                    EndTimeDate2 = end,
                    //shift 3
                    ScheduleGroupId3 = scheduleGroupId,
                    StartTimeDate3 = start,
                    EndTimeDate3 = end,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN1",
                        LastName = "LN1",
                    },
                }
            };

            //ACT
            var groupProvider = new ScheduleGroupProvider(mocks.BusinessApiSessionMoq.Object) as IScheduleGroupProvider;
            var results = new SchedulingProvider.SplitPublishedRawData()
                .Execute(data, null, groupProvider);

            //ASSERT
            Assert.IsNotNull(results);
            CollectionAssert.AllItemsAreInstancesOfType(results, typeof(EmployeeSchedulesDto));
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.First().EmployeeScheduleShifts.Count() == 3);
        }

        [Test]
        public void SplitPublishedRawData3()
        {
            //ARRANGE
            var mocks = new ApiSessionMockBase();
            var start = new DateTime(1900, 1, 1, 8, 0, 0);
            var end = new DateTime(1900, 1, 1, 15, 0, 0);
            var scheduleGroupId = 1;

            var data = new List<ClockEmployeeScheduleShiftDto>()
            {
                new ClockEmployeeScheduleShiftDto()
                {
                    EmployeeId = 1,
                    //shift 1
                    ScheduleGroupId1 = scheduleGroupId,
                    StartTimeDate1 = start,
                    EndTimeDate1 = end,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN1",
                        LastName = "LN1",
                    },
                },
                new ClockEmployeeScheduleShiftDto()
                {
                    EmployeeId = 1,
                    //shift 1
                    ScheduleGroupId1 = scheduleGroupId,
                    StartTimeDate1 = start,
                    EndTimeDate1 = end,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN1",
                        LastName = "LN1",
                    },
                },
                new ClockEmployeeScheduleShiftDto()
                {
                    EmployeeId = 2,
                    //shift 1
                    ScheduleGroupId1 = scheduleGroupId,
                    StartTimeDate1 = start,
                    EndTimeDate1 = end,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN2",
                        LastName = "LN2",
                    },
                },
            };

            //ACT
            var groupProvider = new ScheduleGroupProvider(mocks.BusinessApiSessionMoq.Object) as IScheduleGroupProvider;
            var results = new SchedulingProvider.SplitPublishedRawData()
                .Execute(data, null, groupProvider);

            //ASSERT
            Assert.IsTrue(results.Count() == 2,
                "Result count is incorrect");

            Assert.IsTrue(results.First(x => x.EmployeeId == 1).EmployeeScheduleShifts.Count() == 2,
                "Shift count is incorrect");

            Assert.IsTrue(
                results.First(x => x.EmployeeId == 2).EmployeeScheduleShifts.Count() == 1,
                "Shift count is incorrect");
        }

        [Test]
        public void FlattenData()
        {
            //ARRANGE
            var data = new[]
            {
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN1",
                        LastName = "LN1",
                    },
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 2,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN2",
                        LastName = "LN2",
                    },
                },
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                    EmployeeDataDto = new SchedulesEmployeeDataDto()
                    {
                        FirstName = "FN1",
                        LastName = "LN1",
                    },
                }
            };

            //ACT
            var results = new SchedulingProvider.FlattenScheduleData()
                .Execute(data);

            //ASSERT
            Assert.AreEqual(
                2,
                results.Count(),
                "Count is incorrect");

            Assert.AreEqual(
                2,
                results.First(x => x.EmployeeId == 1).EmployeeScheduleShifts.Count(),
                "Count is incorrect");

            Assert.AreEqual(
                1,
                results.First(x => x.EmployeeId == 2).EmployeeScheduleShifts.Count(),
                "Count is incorrect");

            Assert.IsFalse(
                results.Any(x => x.EmployeeScheduleShifts.Any(y => y.EmployeeDataDto != null)),
                "All employee data should be set to null in the shifts");
        }

        [Test]
        public void ProcessGetShiftData()
        {
            //ARRANGE
            var data = new List<EmployeeSchedulesDto>()
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                    IsTerminated = false,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>()
                    {
                        new ScheduleShiftDto() {EmployeeId = 1}
                    }
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 2,
                    IsTerminated = true,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>(),
                },
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 3,
                    IsTerminated = false,
                    EmployeeScheduleShifts = new List<ScheduleShiftDto>(),
                },
            };

            var recurringShifts = new[]
            {
                new ScheduleShiftDto()
                {
                    EmployeeId = 1,
                },
            };

            var empCC = new[]
            {
                new EmployeeSchedulesDto()
                {
                    EmployeeId = 1,
                },
            };

            //ACT
            new SchedulingProvider.ProcessGetShiftData()
                .Execute(data, recurringShifts, empCC);

            //ASSERT
            Assert.AreEqual(
                2,
                data.Count(),
                "Result count is incorrect");

            Assert.AreEqual(
                1,
                data.SelectMany(x => x.EmployeeRecurringShifts).Count(),
                "Result count is incorrect");
        }

        #endregion

    }

}
