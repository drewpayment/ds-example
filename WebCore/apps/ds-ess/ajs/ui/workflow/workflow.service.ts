import { DsStateService } from '@ajs/core/ds-state/ds-state.service';
import { AccountService } from '@ajs/core/account/account.service';
import { STATES } from '../../shared/state-router-info';

declare var _: any;

/**
 * Class object used to define a navigational menu containing menu-items in the ESS application.
 *
 * @name ds.ess.ui~Menu
 */
export class WorkflowService {
    static readonly SERVICE_NAME = 'Workflow';
    static readonly $inject = ['$q', '$rootScope', '$location',
        DsStateService.SERVICE_NAME, AccountService.SERVICE_NAME, 'DsOnboardingApi'];
    static readonly Icons = {
        pause: 'PAUSE',
        check: 'CHECK',
        circle: 'CIRCLE'
    };

    items = [];
    flatWorkflowList = [];
    employeeId: number;
    userId: number;
    currState = null;
    workflowPageCnt = 0;
    isWorkflowLoaded = false;
    isWorkflowLoading = false;

    constructor(
        private $q: ng.IQService,
        private $rootScope: any,
        private $location: ng.ILocationService,
        private DsState: DsStateService,
        private accountService: AccountService,
        private DsOnboardingApi) {

        $rootScope.hasLicense = true;

        $rootScope.$on('$stateChangeSuccess', (event, toState, toParam) => {
            this.setCurrState(toState, toParam);
        });
    }

    getUserWorkflowPromise(): ng.IPromise<WorkflowService> {
        let deferred = this.$q.defer<WorkflowService>();
        if (!this.isWorkflowLoaded && !this.isWorkflowLoading) {
            this.isWorkflowLoading = true;
            this.accountService.getUserInfo().then((data) => {

                this.employeeId = data.employeeId;
                this.userId = data.userId;

                this.DsOnboardingApi.getWorkflowByEmp(data.employeeId).then((empWorkflow) => {
                    this.isWorkflowLoading = false;
                    this.isWorkflowLoaded = true;

                    _.each(_.filter(empWorkflow.data,x => !x.onboardingTask.mainTaskId ), (workFlow) => {
                        this.createMenuItem(workFlow);
                    });

                    _.each(_.filter(empWorkflow.data,x => !!x.onboardingTask.mainTaskId), (workFlow) => {
                        let menuItem = this.createSubMenuItem(workFlow);
                        this.setMenuState(workFlow.isCompleted
                            ? workFlow.isCompleted
                            : (workFlow.isCompleted === false) ? false : null, menuItem);
                    });

                    this.setMainMenuStatus();
                    this.createFlattenWorkflowList();

                    if (this.checkFinalized()) {
                        this.DsState.router.go(
                            this.flatWorkflowList[this.flatWorkflowList.length - 1].linkToState,
                            this.geParameters(this.flatWorkflowList[this.flatWorkflowList.length - 1]));

                    }

                    deferred.resolve(this);

                });

            });


        } else {
            deferred.resolve(this);
        }

        return deferred.promise;
    }

    checkFinalizeStatusAndNavigate() {
        if (this.checkFinalized()) {
            this.DsState.router.go(
                this.flatWorkflowList[this.flatWorkflowList.length - 1].linkToState,
                this.geParameters(this.flatWorkflowList[this.flatWorkflowList.length - 1]));
        }
    }

    getUserWorkflow() {
        return this.items;
    }

    getCircleNumber() {
        if (this.flatWorkflowList.length === 0) return null;

        if (!this.currState)
            this.currState = (<any>this.$location).$$url;

        return this.flatWorkflowList[this.findIndex(this.currState)].circleNumber;
    }

    getWorkFlowTitle() {
        if (this.flatWorkflowList.length === 0) return null;

        if (!this.currState)
            this.currState = (<any>this.$location).$$url;

        return this.flatWorkflowList[this.findIndex(this.currState)].title;
    }

    getWorkFlowDescription() {
        if (this.flatWorkflowList.length === 0) return null;

        if (!this.currState)
            this.currState = (<any>this.$location).$$url;

        return this.flatWorkflowList[this.findIndex(this.currState)].description;
    }

    getFormDefinitionId() {
        if (this.flatWorkflowList.length === 0) return null;

        if (!this.currState)
            this.currState = (<any>this.$location).$$url;

        const index = this.findIndex(this.currState);

        return this.flatWorkflowList[index].formDefinitionId;
    }

    getFormTypeId() {
        if (this.flatWorkflowList.length === 0) return null;

        if (!this.currState)
            this.currState = (<any>this.$location).$$url;

        const index = this.findIndex(this.currState);

        return this.flatWorkflowList[index].formTypeId;
    }

    getNextPrevPage(state, direction) {

        let index = this.findIndex(state);

        if (index > -1) {
            index = index + direction;

            if ((index >= this.flatWorkflowList.length && direction === 1) || (index <= 0 && direction === -1)) {
                return;
            }

            let workFlow = this.flatWorkflowList[index];

            if (workFlow.isHeader) {
                workFlow = this.flatWorkflowList[index + direction];
            }

            this.DsState.router.go(workFlow.linkToState, this.geParameters(workFlow));
        }
    }

    updateWorkflowTask(state, isComplete) {
        let idx = this.findIndex(state);

        let workFlow = this.flatWorkflowList[idx];
        workFlow.isCompleted = isComplete;
        let workflowTask = this.createWorkFlowTask(this.flatWorkflowList[idx].onboardingWorkflowTaskId, isComplete);

        this.setMenuState(isComplete, this.getMenu(workflowTask.onboardingWorkflowTaskId));

        let pct = this.determineCompletionPct();

        if (isComplete) {
            this.$rootScope.$emit('$itemCompletion', pct);
            this.$rootScope.$emit('$workflowUpdate');
        }

        this.DsOnboardingApi.putWorkflowByEmp(workflowTask).then(() => {
            // Need to check for an error.
            this.setMainMenuStatus();
        });
    }

    determineCompletionPct() {
        let idx;
        let itemCnt = 0;
        let itemCompleteCnt = 0;

        for (idx = 0; idx < this.items.length; idx++) {
            if (this.items[idx].subMenus.length > 0) {
                for (let i = 0; i < this.items[idx].subMenus.length; i++) {
                    itemCnt++;
                    if (this.items[idx].subMenus[i].icon === WorkflowService.Icons.check) {
                        itemCompleteCnt++;
                    }
                }
            } else {
                itemCnt++;

                if (this.items[idx].icon === WorkflowService.Icons.check) {
                    itemCompleteCnt++;
                }
            }
        }  // for

        let pct = itemCompleteCnt === 0 && itemCnt === 0
            ? 0 : Math.round(itemCompleteCnt / itemCnt * 100);
        return pct;

    }

    checkFinalizedStatus() {
        return this.checkFinalized();
    }

    goWorkFlowByMenu(menu) {
        this.DsState.router.go(menu.linkToState, this.geParameters(menu));
    }

    isAllWorkFlowCompleted () {
        let isCompleted = true;
        _.each(this.items, function (item) {
            if (item.linkToState !== 'ess.onboarding.finalize') {
                if (!item.subMenus.length && item.isCompleted != true) {
                    isCompleted = false;
                    return false;
                } else {
                    _.each(item.subMenus, function (subMenu) {
                        if (subMenu.isCompleted != true) {
                            isCompleted = false;
                            return false;
                        }
                    });
                }
            }
        });

        return isCompleted;
    }

    isI9Included = function () {
        let flag = false;
        _.each(this.items, function (item) {
            if (item.linkToState === 'ess.onboarding.i9') {
                flag = true;
            }
        });

        return flag;
    };

    createFlattenWorkflowList() {

        for (let idx = 0; idx < this.items.length; idx++) {

            this.flatWorkflowList.push(this.createWorkflowItem(this.items[idx]));

            if (this.items[idx].subMenus.length > 0) {
                // Multiple Sub-Menus
                for (let i = 0; i < this.items[idx].subMenus.length; i++) {
                    this.flatWorkflowList.push(this.createWorkflowItem(this.items[idx].subMenus[i]));
                }
            }
        }
    }

    checkFinalized() {
        if (this.flatWorkflowList && this.flatWorkflowList.length)
            return this.flatWorkflowList[this.flatWorkflowList.length - 1].isCompleted;
        return false;
    }

    findIndex(state): number {
        return _.findIndex(this.flatWorkflowList, workflow => workflow.getLinkToState() === state || workflow.getUrl() === state);
        // return this.flatWorkflowList.findIndex(workflow => workflow.getLinkToState() === state || workflow.getUrl() === state);
    }

    setCurrState(state, toParam) {
        if (!toParam || !toParam.workFlowTaskId) {
            this.currState = state.name;
        } else {
            this.currState = state.name + '({workFlowTaskId : ' + toParam.workFlowTaskId + '})';
        }
    }

    getCurrState() {

        if (!this.currState) {
            this.currState = this.flatWorkflowList[this.findIndex((<any>this.$location).$$url)].getLinkToState();
        }

        return this.currState;
    }

    geParameters(item) {
        if (item.requireWorkFlowTaskId == true) {
            return { workFlowTaskId: item.onboardingWorkflowTaskId };
        }

        return null;
    }

    getLinkToState(item) {
        if (item.requireWorkFlowTaskId == true) {
            return item.linkToState + '({workFlowTaskId : ' + item.onboardingWorkflowTaskId + '})';
        }

        return item.linkToState;
    }

    getUrl(item) {
        if (item.isHeader) {
            return '';
        }

        if (item.requireWorkFlowTaskId == true) {
            return this.DsState.router.get(item.linkToState).url.replace(':workFlowTaskId', '') + item.onboardingWorkflowTaskId;
        }

        return this.DsState.router.get(item.linkToState).url;
    }

    createWorkflowItem(item) {
        let svc = this;
        let workflowItem = <any>{};

        workflowItem.onboardingWorkflowTaskId = item.onboardingWorkflowTaskId;
        workflowItem.title = item.title;
        workflowItem.linkToState = item.linkToState;
        workflowItem.mainTaskId = item.mainTaskId;
        workflowItem.url = '/onboarding' + item.getUrl();
        workflowItem.isHeader = item.isHeader;
        workflowItem.isCompleted = item.isCompleted;
        workflowItem.formDefinitionId = item.formDefinitionId;
        workflowItem.formTypeId = item.formTypeId;
        workflowItem.description = item.description;
        workflowItem.requireWorkFlowTaskId = item.requireWorkFlowTaskId;
        workflowItem.getLinkToState = function () {
            return svc.getLinkToState(this);
        };

        workflowItem.getUrl = function () {
            return workflowItem.url;
        };

        if (item.isHeader) {
            // Don't count.
        } else {
            this.workflowPageCnt++;
            workflowItem.circleNumber = this.workflowPageCnt;
        }

        return workflowItem;
    }

    createWorkFlowTask(onboardingWorkflowTaskId, isCompleted) {
        return {
            employeeId: this.employeeId,
            isCompleted: isCompleted,
            onboardingWorkflowTaskId: onboardingWorkflowTaskId,
            modified: new Date(),
            modifiedBy: this.userId
        };
    }

    setMainMenuStatus() {
        let idx;

        _.each(this.items, function (item) {
            if (item.subMenus.length > 0) {
                let incompletedOrUnStartedSubMenu = _.find(item.subMenus, function (subMenu) { return subMenu.icon === WorkflowService.Icons.pause || subMenu.icon === WorkflowService.Icons.circle; });
                if (incompletedOrUnStartedSubMenu) {
                    let incompletedOrCompletedSubMenu = _.find(item.subMenus, function (subMenu) { return subMenu.icon === WorkflowService.Icons.pause || subMenu.icon === WorkflowService.Icons.check; });
                    if (incompletedOrCompletedSubMenu) {
                        item.icon = WorkflowService.Icons.pause;
                    } else {
                        item.icon = WorkflowService.Icons.circle;
                    }
                } else {
                    item.icon = WorkflowService.Icons.check;
                }
            } else {
                item.icon = (item.isCompleted == true) ? WorkflowService.Icons.check : ((item.isCompleted === false) ? WorkflowService.Icons.pause : WorkflowService.Icons.circle);
            }
        });
    }

    getMenu(workFlowTaskId) {
        let menu = null;
        _.each(this.items, function (item) {
            if (item.onboardingWorkflowTaskId === workFlowTaskId) {
                menu = item;
                return false;
            }

            if (item.subMenus.length > 0) {
                _.each(item.subMenus, function (subMenu) {
                    if (subMenu.onboardingWorkflowTaskId === workFlowTaskId) {
                        menu = subMenu;
                        return false;
                    }
                });
            }

            if (menu) {
                return false;
            }
        });

        return menu;
    }

    setMenuState(isCompleted, menuItem) {
        menuItem.isCompleted = isCompleted;
        menuItem.icon = isCompleted === true ? WorkflowService.Icons.check : (isCompleted === false) ? WorkflowService.Icons.pause : WorkflowService.Icons.circle;
    }

    createMenuItem(item) {
        let svc = this;
        let menuItem = {
            onboardingWorkflowTaskId: item.onboardingWorkflowTaskId,
            title: item.onboardingTask.workflowTitle,
            linkToState: item.onboardingTask.linkToState,
            mainTaskId: item.onboardingTask.mainTaskId,
            requireWorkFlowTaskId: item.onboardingTask.requireWorkFlowTaskId,
            url: null,
            isHeader: item.onboardingTask.isHeader,
            activeState: item.activeState,
            authorized: item.authorized,
            icon: WorkflowService.Icons.circle,
            toggleState: item.toggleState,
            subMenus: [],
            isCompleted: item.isCompleted,
            formDefinitionId: item.formDefinitionId,
            formTypeId: item.formTypeId,
            description: item.onboardingTask.description,
            getLinkToState: function () {
                return svc.getLinkToState(this);
            },
            getUrl: function () {
                return svc.getUrl(this);
            }
        };

        menuItem.getLinkToState = function () {
            return svc.getLinkToState(this);
        };

        menuItem.getUrl = function () {
            return svc.getUrl(this);
        };

        if (item.onboardingTask.isHeader) {
            // Do Nothing.

        } else {
            menuItem.url = this.DsState.router.get(menuItem.linkToState).url;

        }

        this.items.push(menuItem);

        return menuItem;
    }

    createSubMenuItem(item) {
        let svc = this;
        let subMenu = <any>{
            onboardingWorkflowTaskId: item.onboardingWorkflowTaskId,
            title: item.onboardingTask.workflowTitle,
            linkToState: item.onboardingTask.linkToState,
            mainTaskId: item.onboardingTask.mainTaskId,
            requireWorkFlowTaskId: item.onboardingTask.requireWorkFlowTaskId,
            activeState: item.activeState,
            authorized: item.authorized,
            icon: WorkflowService.Icons.circle,
            isCompleted: item.isCompleted,
            formDefinitionId: item.formDefinitionId,
            formTypeId: item.formTypeId,
            description: item.onboardingTask.description
        };

        subMenu.getLinkToState = function () {
            return svc.getLinkToState(this);
        };

        subMenu.getUrl = function () {
            return svc.getUrl(this);
        };

        subMenu.url = this.DsState.router.get(subMenu.linkToState).url;

        let parentMenu = _.find(this.items, function (menu) { return menu.onboardingWorkflowTaskId === item.onboardingTask.mainTaskId; });

        if (parentMenu) {
            parentMenu.subMenus.push(subMenu);
        }

        return subMenu;
    }

    isStateCompleted(state) {
        let idx = this.findIndex(state);
        return this.flatWorkflowList[idx] && this.flatWorkflowList[idx].isCompleted;
    }
}
