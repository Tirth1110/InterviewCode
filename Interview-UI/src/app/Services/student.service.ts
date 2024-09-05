import { Injectable } from '@angular/core';
import { ResponseModel } from '../Models/ResponseModel';
import { HttpClient } from '@angular/common/http';
import { StudentSubjectForCreateDTO } from '../Models/StudentSubjectForCreateDTO';
import { Observable } from 'rxjs';
import { StudentDetailsImport } from '../Models/StudentDetailsImport';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private API_URL = 'https://localhost:7141'
  controllerName: string = 'Student';

  constructor(private http: HttpClient) { }
  insertUpdate(studentForCreateModel: FormData) {
    return this.http.post<ResponseModel<any>>(`${this.API_URL}/${this.controllerName}/InsertUpdate`, studentForCreateModel);
  }
  getById(studentId: string) {
    return this.http.get<ResponseModel<any>>(`${this.API_URL}/${this.controllerName}/GetById/${studentId}`);
  }
  getAll() {
    return this.http.get<ResponseModel<any>>(`${this.API_URL}/${this.controllerName}/GetAll`);
  }
  insertSubjectMarks(studentSubjectForCreateList: StudentSubjectForCreateDTO[]) {
    return this.http.post<ResponseModel<any>>(`${this.API_URL}/${this.controllerName}/InsertSubjectMarks`, studentSubjectForCreateList);
  }
  exportPDF(studentId: string): Observable<Blob> {
    return this.http.get(`${this.API_URL}/${this.controllerName}/ExportPDF/${studentId}`, { responseType: 'blob' });
  }

  updateStudentDataViaExcel(studentDetailsImportObj: StudentDetailsImport[]) {
    return this.http.put<ResponseModel<any>>(`${this.API_URL}/${this.controllerName}/UploadStudentDataViaExcel`, studentDetailsImportObj);
  }
}
