import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessRuleManagerComponent } from './access-rule-manager.component';

describe('AccessRuleManagerComponent', () => {
  let component: AccessRuleManagerComponent;
  let fixture: ComponentFixture<AccessRuleManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccessRuleManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessRuleManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
