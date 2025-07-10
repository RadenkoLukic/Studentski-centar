A WPF desktop application for managing student dormitory services and administrative tasks. The app is built using the MVVM architectural pattern and connected to a MySQL database.
It provides separate user roles for admin and employees, as well as a public interface for unregistered users.

✨ Key Features:
  🔐 Login and Sign-Up System: Supports registration and authentication of users.

  👥 Role-based Access:
    * Admin can view, add, edit, and delete employee data.
    * Employees can manage student-related data and dorm room allocations.
    * Unregistered Users can browse public content such as announcements and dormitory-related information, but cannot make changes.

📋 Student Data Management: Input and manage student profiles, their place of residence, faculty, and study programs.

📢 Notice Board: Employees can publish and manage announcements relevant to students and dorm staff.

🌐 Multilanguage Support: UI supports both Serbian and English (language toggle).

🎨 Theme Switching: Supports three different themes (Light, Dark, and Green) that can be changed dynamically by the user.

🧱 Technologies Used:
  * WPF (C#)
  * MVVM Architecture
  * MySQL (via MySQL Connector)
  * MaterialDesignThemes.Wpf
  * XAML for UI
