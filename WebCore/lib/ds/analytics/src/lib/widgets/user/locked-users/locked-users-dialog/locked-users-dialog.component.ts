import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UserPerformanceDashboard, UserPerformanceDashboardDialogData } from "@ds/analytics/models";

@Component({
    selector: "ds-locked-users-dialog",
    templateUrl: "./locked-users-dialog.component.html",
    styleUrls: ["./locked-users-dialog.component.css"],
})
export class LockedUsersDialogComponent implements OnInit {
    dataSource: MatTableDataSource<UserPerformanceDashboard>;
    displayedColumns: string[] = ["employeeName", "supervisorName", "userName"];

    constructor(
        @Inject(MAT_DIALOG_DATA) public data: UserPerformanceDashboardDialogData,
        public dialogRef: MatDialogRef<LockedUsersDialogComponent>
    ) {}

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    close(): void {
        this.dialogRef.close();
    }

    ngAfterViewInit() {}

    ngOnInit() {
        this.dataSource = new MatTableDataSource<UserPerformanceDashboard>(
            this.data.users
        );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = (item, sortColumn) => {
            switch (sortColumn) {
                case "employeeName":
                    return item.lastName;
                case "supervisorName":
                    return item.supervisor;
                case "userName":
                    return item.userName;
                default:
                    return item[this.sort.active];
            }
        };
    }

    applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }
    print = () => {
        var employees = this.data.users;

        // if (employees != null) {
        //     employees.sort((a, b) => {
        //         a.firstName > b.firstName ? -1 : 1;
        //     });
        // }

        var printWindow = window.open(
            this.data.title,
            "Print" + new Date().getTime(),
            "width=1200,height=700"
        );
        printWindow.document.write("<!DOCTYPE html><html><head>");

        for (var i = 0; i < document.styleSheets.length; i++) {
            if (document.styleSheets[i].href) {
                printWindow.document.write(
                    '<link href= "' +
                        document.styleSheets[i].href +
                        '" rel="stylesheet" type="text/css" >'
                );
            }
        }

        printWindow.document.write(
            '</head><body style="background-color:#fff;">'
        );
        printWindow.document.write(
            "<style> html{font-size: 12px} @page{size:landscape;}</style>"
        );
        printWindow.document.write('<div class="print-page">');
        printWindow.document.write('<div class="print-page-header">');
        printWindow.document.write(
            '<h1 class="print-header">' + this.data.title + "</h1>"
        );
        printWindow.document.write('<div class="print-page-section-group">');
        if (employees != null) {
            if (employees.length == 1)
                printWindow.document.write(
                    '<div class="instruction-text">' +
                        employees.length +
                        " Employee</div>"
                );
            else {
                printWindow.document.write(
                    '<div class="instruction-text">' +
                        employees.length +
                        " Employees</div>"
                );
            }
        } else {
            printWindow.document.write(
                '<div class="instruction-text">' + "0" + " Employee</div>"
            );
        }
        printWindow.document.write("</div>");
        printWindow.document.write("</div>");
        printWindow.document.write("<table>");

        printWindow.document.write("<tr>");
        printWindow.document.write("<th>Employee Name</th>");
        printWindow.document.write("<th>Supervisor Name</th>");
        printWindow.document.write("<th>User Name</th>");
        printWindow.document.write("</tr>");

        if (employees != null) {
            employees.forEach((employee) => {
                printWindow.document.write("<tr>");
                printWindow.document.write(
                    "<td>" +
                        `${employee.lastName}, ${employee.firstName}` +
                        "</td>"
                );
                printWindow.document.write(
                    "<td>" + employee.supervisor + "</td>"
                );
                printWindow.document.write(
                    "<td>" + employee.userName + "</td>"
                );
                printWindow.document.write("<tr>");
            });
        }

        printWindow.document.write("</table>");
        printWindow.document.write("</div>");
        printWindow.document.write("<script>window.print();</script>");
        printWindow.document.write("</body></html>");
        printWindow.document.close();
        printWindow.focus();

        printWindow.addEventListener("afterprint", (event) => {
            printWindow.close();
        });
    };

    convertDate(date: Date) {
        var months = [
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec",
        ];

        return `${
            months[date.getMonth()]
        } ${date.getDate()}, ${date.getFullYear()}`;
    }
}
