import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArOutletComponent } from './ar-outlet.component';

describe('ArOutletComponent', () => {
  let component: ArOutletComponent;
  let fixture: ComponentFixture<ArOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
