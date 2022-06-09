import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavDocsComponent } from './sidenav-docs.component';

describe('SidenavDocsComponent', () => {
  let component: SidenavDocsComponent;
  let fixture: ComponentFixture<SidenavDocsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SidenavDocsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SidenavDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
