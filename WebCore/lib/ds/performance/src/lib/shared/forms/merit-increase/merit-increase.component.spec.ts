import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MeritIncreaseComponent } from './merit-increase.component';

describe('MeritIncreaseComponent', () => {
  let component: MeritIncreaseComponent;
  let fixture: ComponentFixture<MeritIncreaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MeritIncreaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MeritIncreaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
