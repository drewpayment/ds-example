import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppResourceManagerComponent } from './app-resource-manager.component';

describe('AppResourceManagerComponent', () => {
  let component: AppResourceManagerComponent;
  let fixture: ComponentFixture<AppResourceManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppResourceManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppResourceManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
