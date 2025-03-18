import { FormsModule } from '@angular/forms'; // Import FormsModule
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NavComponent } from "../nav/nav.component";

@Component({
 selector: 'app-manage-category',
 imports: [CommonModule, FormsModule, NavComponent],
 templateUrl: './manage-category.component.html',
 styleUrls: ['./manage-category.component.css'],
})
export class ManageCategoryComponent implements OnInit {
 categories: any[] = [];
 isLoading = true;
 errorMessage = '';
 successMessage = '';
 categoryForm: any = {
   categoryId: null,
   name: '',
   isActive: true,
 };
 private baseUrl = 'https://localhost:44326/api/Category';
 constructor(private http: HttpClient) {}
 ngOnInit(): void {
   this.fetchCategories();
 }
 fetchCategories(): void {
   this.isLoading = true;
   this.http.get<any[]>(`${this.baseUrl}`).subscribe({
     next: (categories) => {
       this.categories = categories;
       this.isLoading = false;
     },
     error: (error) => {
       this.errorMessage = 'Failed to load categories.';
       console.error('Error fetching categories:', error);
       this.isLoading = false;
     },
   });
 }
 onSubmitCategoryForm(): void {
   if (this.categoryForm.categoryId) {
     this.updateCategory();
   } else {
     this.addCategory();
   }
 }
 addCategory(): void {
   this.http.post(`${this.baseUrl}`, this.categoryForm).subscribe({
     next: () => {
       this.successMessage = 'Category added successfully!';
       this.fetchCategories();
       this.resetForm();
     },
     error: (error) => {
       this.errorMessage = 'Failed to add category.';
       console.error('Error adding category:', error);
     },
   });
 }
 updateCategory(): void {
   const url = `${this.baseUrl}/${this.categoryForm.categoryId}`;
   this.http.put(url, this.categoryForm).subscribe({
     next: () => {
       this.successMessage = 'Category updated successfully!';
       this.fetchCategories();
       this.resetForm();
     },
     error: (error) => {
       this.errorMessage = 'Failed to update category.';
       console.error('Error updating category:', error);
     },
   });
 }
 deleteCategory(categoryId: number): void {
   const url = `${this.baseUrl}/${categoryId}`;
   this.http.delete(url).subscribe({
     next: () => {
       this.successMessage = 'Category deleted successfully!';
       this.fetchCategories();
     },
     error: (error) => {
       this.errorMessage = 'Failed to delete category.';
       console.error('Error deleting category:', error);
     },
   });
 }
 editCategory(category: any): void {
   this.categoryForm = { ...category };
 }
 resetForm(): void {
   this.categoryForm = {
     categoryId: null,
     name: '',
     isActive: true,
   };
 }
}