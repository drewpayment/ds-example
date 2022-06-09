using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ApplicantSkillMapper
    {
        public class ToApplicantSkill : ExpressionMapper<ApplicantSkillDto, ApplicantSkill>
        {
            public override Expression<Func<ApplicantSkillDto, ApplicantSkill>> MapExpression
            {
                get
                {
                    return SkillDto => new ApplicantSkill()
                    {
                        ApplicantSkillId = SkillDto.ApplicantSkillId,
                        ApplicantId = SkillDto.ApplicantId,
                        Name = SkillDto.Name,
                        Experience = SkillDto.Experience,
                        ExperienceType = SkillDto.ExperienceType,
                        Rating = SkillDto.Rating,
                        IsEnabled = SkillDto.IsEnabled,
                    };
                }
            }
        }

        public class ToApplicantSkillDto : ExpressionMapper<ApplicantSkill, ApplicantSkillDto>
        {
            public override Expression<Func<ApplicantSkill, ApplicantSkillDto>> MapExpression
            {
                get
                {
                    return Skill => new ApplicantSkillDto()
                    {
                        ApplicantSkillId = Skill.ApplicantSkillId,
                        ApplicantId = Skill.ApplicantId,
                        Name = Skill.Name,
                        Experience = Skill.Experience,
                        ExperienceType = Skill.ExperienceType,
                        Rating = Skill.Rating,
                        IsEnabled = Skill.IsEnabled,
                    };
                }
            }
        }
    }
}
