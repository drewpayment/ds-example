import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessRuleSetEditorComponent } from './access-rule-set-editor.component';

describe('AccessRuleSetEditorComponent', () => {
  let component: AccessRuleSetEditorComponent;
  let fixture: ComponentFixture<AccessRuleSetEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccessRuleSetEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessRuleSetEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
