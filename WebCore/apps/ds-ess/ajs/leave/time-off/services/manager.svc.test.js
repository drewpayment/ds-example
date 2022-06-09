describe("ds.ess.leave:TimeOffManagerService | ", function () {
    var managerService, mocks;

    beforeEach(module("ds.ess.leave", function ($provide) {
        mocks = {};
        mocks.TimeOffManagerInstance = function () {};
        $provide.value('TimeOffManagerInstance', mocks.TimeOffManagerInstance);
    }));

    beforeEach(inject(function ($injector) {
        managerService = $injector.get('TimeOffManagerService');
    }));

    xdescribe('manageUserTimeOff() | ', function () {
        it("should create a new instance", function () {
            var manager = managerService.manageUserTimeOff();
            expect(typeof manager).toBe(typeof new mocks.TimeOffManagerInstance());
        });
    });
});

describe("ds.ess.leave:TimeOffManagerInstance | ", function () {
    var TimeOffManagerInstance, mocks, $rootScope;
    
    beforeEach(function () {
        module('ds.ess.leave');
        module('ds.core.mocks');
        module('ds.leave.mocks');

        mocks = {};
        inject(function($injector) {
            TimeOffManagerInstance = $injector.get('TimeOffManagerInstance'); 
            $rootScope = $injector.get('$rootScope');

            mocks.DsTimeOffService = $injector.get('DsTimeOffService');
            mocks.AccountService = $injector.get('AccountService');
        });
    });

    it('should be defined', function() {
        expect(TimeOffManagerInstance).toBeDefined(); 
    });

    describe('Initialization | ', function() {
        
        it("should return a promise", function () {
            var user = { employeeId: 100 };
            var manager = new TimeOffManagerInstance(user);

            expect(manager.then).toBeDefined();
        });

        it("should resolve with the manager definition after all API calls resolve", function () {
            var user = { employeeId: 100 };
            var manager;

            new TimeOffManagerInstance(user).then(function(mgr){ manager = mgr; });

            mocks.DsTimeOffService.getTimeOffPolicyActivity.deferred.resolve([]);
            mocks.DsTimeOffService.getHistoricalReports.deferred.resolve([]);
            mocks.DsTimeOffService.getTimeOffUnitTypes.deferred.resolve([]);
            mocks.DsTimeOffService.getTimeOffAwardTypes.deferred.resolve([]);
            mocks.DsTimeOffService.getTimeOffEventStatusTypes.deferred.resolve([]);

            $rootScope.$digest();

            expect(manager).toBeDefined();
        });

        it("should load user's policies", function () {
            var policies = [{ policyId: 100 }, { policyId: 200 }];

            spyOn(mocks.DsTimeOffService, 'getTimeOffPolicyActivity').and.callThrough();

            var manager = buildManager({policies: policies});

            expect(mocks.DsTimeOffService.getTimeOffPolicyActivity).toHaveBeenCalled();
            expect(manager.policies.length).toBe(policies.length);
        });


        function buildManager(mockData) {
            var manager,
                user = mockData.user || { employeeId: 100 };
            
            new TimeOffManagerInstance(user).then(function(mgr){ manager = mgr; });

            mocks.DsTimeOffService.getTimeOffPolicyActivity.deferred.resolve(mockData.policies || []);
            mocks.DsTimeOffService.getHistoricalReports.deferred.resolve(mockData.historicalReports || []);
            mocks.DsTimeOffService.getTimeOffUnitTypes.deferred.resolve(mockData.timeOffUnitTypes || []);
            mocks.DsTimeOffService.getTimeOffAwardTypes.deferred.resolve(mockData.timeOffAwardTypes || []);
            mocks.DsTimeOffService.getTimeOffEventStatusTypes.deferred.resolve(mockData.eventStatusTypes || []);

            $rootScope.$digest();

            return manager;
        }
    });
});