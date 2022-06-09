import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectronicConsentsComponent } from './electronic-consents.component';

describe('ElectronicConsentsComponent', () => {
  let component: ElectronicConsentsComponent;
  let fixture: ComponentFixture<ElectronicConsentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ElectronicConsentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ElectronicConsentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
