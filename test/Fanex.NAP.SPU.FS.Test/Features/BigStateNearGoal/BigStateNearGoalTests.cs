using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.BigStateNearGoal
{
    public class BigStateNearGoalTests
    {
        private readonly BigStateNearGoalServices bigStateNearGoalServices;
        private readonly IBigStateNearGoalDataAccess bigStateNearGoalDataAccess;

        public BigStateNearGoalTests()
        {
            bigStateNearGoalDataAccess = Substitute.For<IBigStateNearGoalDataAccess>();

            bigStateNearGoalServices = new BigStateNearGoalServices(bigStateNearGoalDataAccess);
        }

        [Fact]
        public void PreCondition_Successful_ReturnMatchList()
        {
            var matchEntity = new List<MatchEntity> {
                new MatchEntity
                {
                    MatchId = 1
                },
                new MatchEntity
                {
                    MatchId =2
                },
                new MatchEntity
                {
                    MatchId = 3
                }
            };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matchEntity);

            bigStateNearGoalServices.Precondition();

            bigStateNearGoalServices.MatchList.Should().BeEquivalentTo(matchEntity);
        }

        [Fact]
        public void PreCondition_Successful_ReturnEmpty()
        {
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());

            bigStateNearGoalServices.Precondition();

            bigStateNearGoalServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_Successful_ReturnEmpty()
        {
            var matchEntity = new List<MatchEntity> {
                new MatchEntity
                {
                    MatchId = 1
                },
                new MatchEntity
                {
                    MatchId =2
                },
                new MatchEntity
                {
                    MatchId = 3
                }
            };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matchEntity);

            bigStateNearGoalServices.Precondition().CleanResult();

            bigStateNearGoalServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void ExecuteRule_Successful_ReturnDataResult()
        {
            var matchEntity = new List<MatchEntity> {
                new MatchEntity
                {
                    MatchId = 1
                },
                new MatchEntity
                {
                    MatchId =2
                },
                new MatchEntity
                {
                    MatchId = 3
                }
            };

            var dataResult = new DataResult { ErrorCode = 0 };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matchEntity);
            bigStateNearGoalServices.Precondition();

            foreach (var match in bigStateNearGoalServices.MatchList)
            {
                bigStateNearGoalDataAccess.Rule(match.MatchId.ToString()).Returns(dataResult);
            }

            var result = bigStateNearGoalServices.Rule();

            result.ErrorCode.Should().Be(0);
        }

        [Fact]
        public void ExecuteRule_Failed_ReturnDataResult()
        {
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());

            var result = bigStateNearGoalServices.Rule();

            result.ErrorCode.Should().Be(-1);
        }

        [Fact]
        public void ExecuteRule_Failed_ReturnException()
        {
            var dataResult = new DataResult { ErrorCode = -100 };

            var matchEntity = new List<MatchEntity> {
                new MatchEntity
                {
                    MatchId = 1
                },
                new MatchEntity
                {
                    MatchId =2
                },
                new MatchEntity
                {
                    MatchId = 3
                }
            };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matchEntity);

            bigStateNearGoalServices.Precondition();

            foreach(var match in bigStateNearGoalServices.MatchList)
            {
                bigStateNearGoalDataAccess.Rule(match.MatchId.ToString()).Returns(dataResult);
            }            

            var result = bigStateNearGoalServices.Rule();

            result.Should().BeEquivalentTo(dataResult);
        }
    }
}
