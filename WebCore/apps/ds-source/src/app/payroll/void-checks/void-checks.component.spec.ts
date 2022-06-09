import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VoidChecksComponent } from './void-checks.component';

describe('VoidChecksComponent', () => {
  let component: VoidChecksComponent;
  let fixture: ComponentFixture<VoidChecksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VoidChecksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoidChecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
