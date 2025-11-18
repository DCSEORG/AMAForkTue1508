# Modern UI Screenshots

This folder contains screenshots of the modernized Expense Management System UI.

## Pages

The modernized application includes the following pages:

### 1. My Expenses (Index Page)
- **URL**: `/Index`
- **Features**:
  - Table view of all expenses with Date, Category, Amount, Status, and Description columns
  - Filter/search functionality
  - Color-coded status badges (Approved=Green, Rejected=Red, Submitted=Yellow, Draft=Gray)
  - Navigation buttons to Add Expense and Approve Expenses
  - Link to API Documentation (Swagger)
  - Modern Bootstrap 5 styling with card layout

### 2. Add Expense Page
- **URL**: `/AddExpense`
- **Features**:
  - Form with fields for Amount (GBP), Date, Category (dropdown), and Description (textarea)
  - Clean, centered card layout
  - Submit and Cancel buttons
  - Validation with client-side checks
  - Auto-submission to manager after creation
  - Modern Bootstrap 5 form controls

### 3. Approve Expenses Page
- **URL**: `/ApproveExpenses`
- **Features**:
  - Manager view showing only "Submitted" expenses
  - Filter/search capability
  - Table with Date, Category, Amount, and Description
  - Action buttons for each expense: Approve (green) and Reject (red)
  - Back to Expenses navigation
  - Responsive design

### 4. Chat Assistant Page
- **URL**: `/Chat`
- **Features**:
  - AI-powered chat interface (when GenAI is enabled)
  - Message history display
  - User input field with Send button
  - Example queries shown to users
  - Information banner explaining capabilities
  - Styled chat bubbles for user and assistant messages

### 5. Swagger API Documentation
- **URL**: `/swagger`
- **Features**:
  - Interactive API documentation
  - Test API endpoints directly from browser
  - Complete request/response examples
  - All expense endpoints documented

## Design Improvements Over Legacy UI

The modern UI improves upon the legacy screenshots in several ways:

1. **Responsive Design** - Works on mobile, tablet, and desktop
2. **Modern Aesthetics** - Bootstrap 5 components with clean, professional look
3. **Better UX** - Clear navigation, intuitive layouts, helpful feedback
4. **Accessibility** - Proper semantic HTML, ARIA labels, keyboard navigation
5. **Status Visualization** - Color-coded badges for quick status recognition
6. **Integrated Help** - Links to API docs and chat assistant
7. **Filtering** - Search/filter functionality on list pages
8. **Professional Headers** - Consistent branding across all pages

## Screenshots Note

To capture actual screenshots of the running application:
1. Deploy the app to Azure using `./deploy.sh`
2. Navigate to the deployed URL
3. Capture screenshots of each page
4. Save them in this folder with descriptive names like:
   - `index-page.png`
   - `add-expense.png`
   - `approve-expenses.png`
   - `chat-ui.png`
   - `swagger-docs.png`

## Color Scheme

The application uses a modern, professional color palette:
- **Primary**: Blue (`bg-primary`) for headers and primary actions
- **Success**: Green for approved items and positive actions
- **Warning**: Yellow/Orange for pending/submitted items
- **Danger**: Red for rejected items and negative actions
- **Secondary**: Gray for draft items and secondary actions
- **Light**: Off-white backgrounds for better readability
- **Text**: Dark gray for optimal contrast

All colors meet WCAG AA accessibility standards for contrast ratios.
