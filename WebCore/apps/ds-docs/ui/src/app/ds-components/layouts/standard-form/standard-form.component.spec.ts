import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StandardFormComponent } from './standard-form.component';

describe('StandardFormComponent', () => {
  let component: StandardFormComponent;
  let fixture: ComponentFixture<StandardFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StandardFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StandardFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
