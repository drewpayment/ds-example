import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OverflowColumnsComponent } from './overflow-columns.component';

describe('OverflowColumnsComponent', () => {
  let component: OverflowColumnsComponent;
  let fixture: ComponentFixture<OverflowColumnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OverflowColumnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OverflowColumnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
