import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchiveOutletComponent } from './archive-outlet.component';

describe('ArchiveOutletComponent', () => {
  let component: ArchiveOutletComponent;
  let fixture: ComponentFixture<ArchiveOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArchiveOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArchiveOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
