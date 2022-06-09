import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationClientComponent } from './organization-client.component';

describe('OrganizationClientComponent', () => {
  let component: OrganizationClientComponent;
  let fixture: ComponentFixture<OrganizationClientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationClientComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
