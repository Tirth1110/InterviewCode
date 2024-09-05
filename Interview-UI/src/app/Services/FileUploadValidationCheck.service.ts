import { Injectable } from '@angular/core';
import { FileExtensions } from '../Common/FileExtensions';

@Injectable({
  providedIn: 'root'
})
export class FileUploadValidationCheckService {
  fileExtensionEnum = FileExtensions;

  constructor() { }

  fileExtensionValidateForImage(fileExtension?: string) {
    return (fileExtension === this.fileExtensionEnum.PNG || fileExtension === this.fileExtensionEnum.JPEG || fileExtension === this.fileExtensionEnum.JPG || fileExtension === this.fileExtensionEnum.GIF);
  }

  fileExtensionValidateForExcel(fileExtension?: string) {
    return (fileExtension === this.fileExtensionEnum.XLS || fileExtension === this.fileExtensionEnum.XLSX || fileExtension === this.fileExtensionEnum.CSV);
  }

  getFileExtension(filename: any) {
    const file: File = filename.target.files[0];
    return file.name.split('.').pop()?.toLowerCase()
  }
}