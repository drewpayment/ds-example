using System;
using System.Collections;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Defines an entity that represents the [dbo].ClientJobCosting table.
    /// 
    /// A job costing is simply an instance of an "entity group". (See the remarks on this class)
    /// The big diferences between a job costing and entity group is that job costings contain some 
    /// additional settings related to the UI of the entity group. For example, IsPostBack, HideOnScreen, and Width all
    /// relate to how the user interacts Associates Assignments with it.
    /// </summary>
    /// <remarks>
    /// Job costing has a tricky table structure, but the premise is simple:
    /// 
    /// Allow a punch to be associated with one or more _entities_ based on _assignments_ that occur on another screen.
    /// 
    /// These assignments define how the entities relate to each other, which in turn enable reporting based on amounts of work done related to those entities.
    /// 
    /// ## Definitions:
    /// - Entity Type:  An identifier that provides a single name for an entity group.
    /// - Entity Group: A set of entities that are all named by a single entity type.
    /// - Job Costing:  An entity that describes a single entity group that can be used. (Represented by this entity)
    /// - Assignment:   An entity that provides a relationship between an entity (A) and a Job Costing (B). 
    ///                 The entity group of (A) must be the same as the entity group of (B).
    /// - Selection:    An entity that provides a relationship between an entity (A) and a Assignment (B).
    ///                 The entity group of (A) must be different from the entity group of (B). 
    ///                 Or in other words, (A) must not have a relationship in (B).
    /// - Association:  An entity that provides a relationship between a punch and one or more selections.
    /// 
    /// Now obviously, there's more to the the system than that, but that's the gist.
    /// 
    /// ## Entity Types
    /// There are currently 6 different types of entities supported:
    /// - Divisions
    /// - Departments
    /// - Cost Centers
    /// - Groups
    /// - Employees
    /// - Custom
    /// 
    /// ## Entity Groups
    /// A client can have up to 6 entity groups. 
    /// Each entity group has an entity type, which defines what actual _type_ of entities the group contains.
    /// For example, an entity group that has the "Division" entity type will contain ClientDivision entities.
    /// With the custom entity type, clients are able to assign custom descriptions and codes as the entity.
    /// Entity groups are related to each other by their level. The level determines how assignments affect possible associations.
    /// The level is simply a number, and entity groups that have lower numbers affect possible assignments for entity groups with lower numbers.
    /// 
    /// ## Job Costing
    /// A job costing is simply an instance of an entity group.
    /// The big diferences between a job costing and entity group is that job costings contain some 
    /// additional settings related to the UI of the entity group. For example, IsPostBack, HideOnScreen, and Width all
    /// relate to how the user interacts Associates Assignments with it.
    /// 
    /// ## Assignment
    /// Assignments are relationships between entities and entity groups.
    /// They are more of an implementation detail, but they are a convenient way to lookup a list of entities for a Job Costing.
    /// 
    /// ## Selection
    /// Selections are relationships between an assignment from one entity group and a assignment from another entity group.
    /// tl;dr Selections limit associations in entity groups.
    /// Essentially, they allow admins to select which assignments they want to be associatable
    /// in a group underneath. For example, if a client has three Job Costings: Location, Department, & Cost Center, then
    /// selections at the Location level can be made for Department and Cost Center. On the other hand, the Department level
    /// can only make selections for the cost center level.
    /// 
    /// ## Association
    /// Associations are relationships between an assignment and a time clock entity such as a punch or benefit.
    /// These are what users decide on the job costing inputs. The possible associations are limited by what was associated at higher levels and what
    /// selections have been made for those levels. For example, given the following scenario:
    /// 
    /// Locations:
    ///     Level: 0
    ///     Assignments:
    ///         - Detroit
    ///             Selections:
    ///                 - Comerica Park
    ///         - LA
    ///             Selections:
    ///                 - Comerica Park
    ///                 - Angel Stadium
    /// Cost Centers:
    ///     Level: 1
    ///     Assignments:
    ///         - Comerica Park
    ///         - Angel Stadium
    /// 
    /// When a user chooses Detroit as the first job costing association, Comerica Park would be the only available selection for the second association.
    /// Whereas when LA is selected, both Comerica Park and Angel Stadium are assocatiable.
    /// 
    /// 
    /// # Stored Procs
    /// There are several stored procedures with make consuming the job costing API significantly easier:
    /// 
    /// - spCheckEmployeeHasJobCosting; Determines if a Client ID, Employee ID, and Source Type (Usually "LABOR") is able to use job costing.
    /// - spGetClientJobCostingList; Gets a list of Job Costing entities for the client.
    /// - spGetClientJobCostingIDWithPostBack; Gets a list of ClientJobCosting IDs that represent the list of ClientJobCosting entities that the given ClientJobCosting ID depends on.
    /// - spGetEmployeeJobCostingAssignmentList_Search; Gets a list of possible associations for the given client ID, Employee ID, and Client Job Costing ID, based on the given comma separated string list of parent Job Costing IDs and comma separated string list of current assignments for those.
    /// 
    /// </remarks>
    public class ClientJobCosting : Entity<ClientJobCosting>
    {
        public int ClientJobCostingId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public byte JobCostingTypeId { get; set; }
        public byte Sequence { get; set; }
        public bool IsEnabled { get; set; }
        public bool HideOnScreen { get; set; }
        public bool IsPostBack { get; set; }
        public double Width { get; set; }
        public byte Level { get; set; }
        public bool IsRequired { get; set; }

        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public ClientJobCostingType JobCostingType => (ClientJobCostingType)JobCostingTypeId;
        public virtual IList<ClientJobCostingAssignment> Assignments { get; set; }
        public virtual IList<ClientJobCostingAssignmentSelected> Selections { get; set; }
        public virtual Client Client { get; set; }
    }
}
