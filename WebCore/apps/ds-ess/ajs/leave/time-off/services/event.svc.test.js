describe("ds.ess.leave:TimeOffEvent | ", function () {
    var TimeOffEvent;

    beforeEach(module('ds.ess.leave'));
    
    beforeEach(inject(function($injector){
        TimeOffEvent = $injector.get('TimeOffEvent');
    }));

    describe("isPending() | ", function() {
        it("should be true when event is 'Request' and 'Pending'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = truthy;
            event.timeOffStatus = 1; // PENDING

            expect(event.isPending()).toBe(true);
        });

        it("should be false when event is NOT 'Request'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = falsy;
            event.timeOffStatus = 1; // PENDING

            expect(event.isPending()).toBe(false);
        });

        it("should be false when event is 'Request' and NOT 'Pending'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = truthy;
            event.timeOffStatus = null; 

            expect(event.isPending()).toBe(false);
        });
    });

     describe("isApproved() | ", function() {
        it("should be true when event is 'Request' and 'Approved'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = truthy;
            event.timeOffStatus = 2; // APPROVED

            expect(event.isApproved()).toBe(true);
        });

        it("should be false when event is NOT 'Request'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = falsy;
            event.timeOffStatus = 2; // APPROVED

            expect(event.isApproved()).toBe(false);
        });

        it("should be false when event is 'Request' and NOT 'Approved'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = truthy;
            event.timeOffStatus = null; 

            expect(event.isApproved()).toBe(false);
        });
    });

    describe("isRejected() | ", function() {
        it("should be true when event is 'Request' and 'Rejected'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = truthy;
            event.timeOffStatus = 3; // REJECTED

            expect(event.isRejected()).toBe(true);
        });

        it("should be false when event is NOT 'Request'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = falsy;
            event.timeOffStatus = 3; // REJECTED

            expect(event.isRejected()).toBe(false);
        });

        it("should be false when event is 'Request' and NOT 'Approved'", function () {
            var event = new TimeOffEvent();
            event.isRequestEvent = truthy;
            event.timeOffStatus = null; 

            expect(event.isRejected()).toBe(false);
        });
    });

    describe("isProjected() | ", function() {
        it("should be alias to isAwardEvent()", function() {
            var event = new TimeOffEvent();
            
            event.isAwardEvent = truthy;
            expect(event.isProjected()).toBe(true);

            event.isAwardEvent = falsy;
            expect(event.isProjected()).toBe(false);
        });
    });

    describe("isAwardEvent() | ", function() {
        it("should be true when timeOffAward has been specified", function() {
            var event = new TimeOffEvent();
            event.timeOffAward = 1;
            expect(event.isAwardEvent()).toBe(true);
        });
        it("should be false when timeOffAward is undefined", function() {
            var event = new TimeOffEvent();
            delete event.timeOffAward;
            expect(event.isAwardEvent()).toBe(false);
        });
        it("should be false when timeOffAward is 0", function() {
            var event = new TimeOffEvent();
            event.timeOffAward = 0;
            expect(event.isAwardEvent()).toBe(false);
        });
    });

    describe("isRequestEvent() | ", function() {
        it("should be true when requestTimeOffId has been specified", function() {
            var event = new TimeOffEvent();
            event.requestTimeOffId = 1;
            expect(event.isRequestEvent()).toBe(true);
        });
        it("should be false when requestTimeOffId is undefined", function() {
            var event = new TimeOffEvent();
            delete event.requestTimeOffId;
            expect(event.isRequestEvent()).toBe(false);
        });
        it("should be false when requestTimeOffId is 0", function() {
            var event = new TimeOffEvent();
            event.requestTimeOffId = 0;
            expect(event.isRequestEvent()).toBe(false);
        });
    });

    describe("getEventStatusType() | ", function () {
        it("should return a 'Projected' status when event is an Award", function () {
            var event = new TimeOffEvent();
            event.isAwardEvent = truthy;
            
            var status = event.getEventStatusType();
            expect(status.eventStatusTypeId).toBe(null);
            expect(status.name).toBe('Projected');
        });
        it("should lookup the status from the manager", function () {
            var manager = { getEventStatusTypeById: angular.noop };
            spyOn(manager, "getEventStatusTypeById");

            var event = new TimeOffEvent({}, { manager: manager });
            event.isAwardEvent = falsy;
            
            event.getEventStatusType();
            expect(manager.getEventStatusTypeById).toHaveBeenCalledWith(event.timeOffStatus);
        });
    });

    describe("getEventType() | ", function () {
        it("should be an Award when timeOffAward is 1", function () {
            var event = new TimeOffEvent();
            event.isAwardEvent = truthy;
            event.timeOffAward = 1;
            
            var eventType = event.getEventType();
            expect(eventType.name).toBe("Award");
        });

        it("should be an Adjustment when timeOffAward is 4", function () {
            var event = new TimeOffEvent();
            event.isAwardEvent = truthy;
            event.timeOffAward = 4;
            
            var eventType = event.getEventType();
            expect(eventType.name).toBe("Adjustment");
        });

        it("should be an Expiration when timeOffAward is not 1 or 4", function () {
            var event = new TimeOffEvent();
            event.isAwardEvent = truthy;
            event.timeOffAward = 2;
            
            var eventType = event.getEventType();
            expect(eventType.name).toBe("Expiration");
        });

        it("should be an Request when not award event and is request event", function () {
            var event = new TimeOffEvent();
            event.isAwardEvent = falsy;
            event.isRequestEvent = truthy;
            
            var eventType = event.getEventType();
            expect(eventType.name).toBe("Request");
        });

        it("should be an Balance when not award event and is not request event", function () {
            var event = new TimeOffEvent();
            event.isAwardEvent = falsy;
            event.isRequestEvent = falsy;
            
            var eventType = event.getEventType();
            expect(eventType.name).toBe("Balance");
        });
    });

    function truthy() {
        return true;
    }

    function falsy() {
        return false;
    }
});