import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlCustomClassTriggerComponent } from './client-gl-custom-class-trigger.component';

describe('ClientGlCustomClassTriggerComponent', () => {
  let component: ClientGlCustomClassTriggerComponent;
  let fixture: ComponentFixture<ClientGlCustomClassTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlCustomClassTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlCustomClassTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
