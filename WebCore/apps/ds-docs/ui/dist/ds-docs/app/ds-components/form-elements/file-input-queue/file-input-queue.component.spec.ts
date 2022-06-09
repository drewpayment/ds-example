import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileInputQueueComponent } from './file-input-queue.component';

describe('FileInputQueueComponent', () => {
  let component: FileInputQueueComponent;
  let fixture: ComponentFixture<FileInputQueueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileInputQueueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileInputQueueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
