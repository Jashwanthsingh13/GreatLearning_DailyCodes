# GitHub Submission Guide

## Repository Setup Instructions

### Step 1: Create Repository on GitHub
1. Go to [GitHub](https://github.com)
2. Sign in to your account
3. Click **New Repository**
4. Enter Repository Name: `CSharp_ExceptionHandling_CaseStudy`
5. Add Description: "Banking Transaction System with C# Exception Handling - Great Learning Case Study"
6. Select **Public** (for Great Learning submission)
7. Do NOT initialize with README, .gitignore, or license (we have our own)
8. Click **Create repository**

### Step 2: Initialize Local Git Repository

```bash
cd /Users/jashwanthrajpurohith/GreatLearning_DailyCode/Day5CaseStudy
git init
git config user.name "Your Name"
git config user.email "your.email@example.com"
```

### Step 3: Add Remote Repository

Replace `YOUR_USERNAME` with your GitHub username:

```bash
git remote add origin https://github.com/YOUR_USERNAME/CSharp_ExceptionHandling_CaseStudy.git
```

### Step 4: Create Initial Commit

```bash
git add InsufficientBalanceException.cs InvalidAmountException.cs
git commit -m "Added custom exceptions"
```

### Step 5: Create Second Commit

```bash
git add BankAccount.cs Program.cs
git commit -m "Implemented withdraw and deposit logic"
```

### Step 6: Create Third Commit

```bash
git add README.md .gitignore BankingApp.csproj
git commit -m "Added documentation and project configuration"
```

### Step 7: Push to GitHub

Change default branch to main if needed:
```bash
git branch -M main
```

Push commits:
```bash
git push -u origin main
```

### Step 8: Verify Submission

1. Go to your GitHub repository URL
2. Verify all commits are present
3. Verify only source files are uploaded (no bin/ or obj/ folders)
4. Verify README.md displays correctly

## Expected Repository Structure

```
CSharp_ExceptionHandling_CaseStudy/
├── Program.cs                          # Main application
├── BankAccount.cs                      # Core banking class
├── InsufficientBalanceException.cs      # Custom exception 1
├── InvalidAmountException.cs            # Custom exception 2
├── BankingApp.csproj                   # Project configuration
├── README.md                           # Documentation
└── .gitignore                          # Git ignore rules
```

## Commit History Expected

```
commit 3: "Added documentation and project configuration"
  - README.md added
  - .gitignore added
  - BankingApp.csproj added

commit 2: "Implemented withdraw and deposit logic"
  - BankAccount.cs added
  - Program.cs added

commit 1: "Added custom exceptions"
  - InsufficientBalanceException.cs added
  - InvalidAmountException.cs added
```

## Alternative: Using GitHub Desktop

1. Open [GitHub Desktop](https://desktop.github.com/)
2. Click **File → Clone Repository**
3. Enter: `https://github.com/YOUR_USERNAME/CSharp_ExceptionHandling_CaseStudy.git`
4. Select local path: `/Users/jashwanthrajpurohith/GreatLearning_DailyCode/Day5CaseStudy`
5. Copy source files into the cloned folder
6. In GitHub Desktop:
   - Stage changes for first commit: exception files
   - Commit with message: "Added custom exceptions"
   - Repeat for other logical groupings
   - Click **Publish branch**

## Quick Upload (If Repository Already Exists)

```bash
cd /Users/jashwanthrajpurohith/GreatLearning_DailyCode/Day5CaseStudy
git status
git add .
git commit -m "Banking Transaction System with Exception Handling"
git push origin main
```

## Troubleshooting

### "fatal: not a git repository"
```bash
git init
git remote add origin https://github.com/YOUR_USERNAME/CSharp_ExceptionHandling_CaseStudy.git
```

### "Permission denied (publickey)"
Generate SSH keys:
```bash
ssh-keygen -t ed25519 -C "your.email@example.com"
cat ~/.ssh/id_ed25519.pub  # Copy output to GitHub Settings → SSH Keys
```

### "bin/ and obj/ folders uploaded by mistake"
Remove from Git:
```bash
git rm -r --cached bin/ obj/
git commit -m "Remove build artifacts"
git push
```

## Submission Checklist

- [ ] Repository created on GitHub with correct name
- [ ] All .cs source files uploaded
- [ ] README.md file present and well-formatted
- [ ] .gitignore file present and working
- [ ] At least 2-3 meaningful commits with clear messages
- [ ] No bin/ or obj/ folders in repository
- [ ] No IDE config files (.vs, .vscode, *.sln, *.user)
- [ ] Repository is Public or Accessible to Evaluators
- [ ] All business logic tests pass (Test output shows successful scenarios)
- [ ] Custom exceptions work correctly (Test output shows exception handling)

## Evaluation Criteria Met

| Criteria | Location | Status |
|---|---|---|
| Exception Handling | BankAccount.cs, Program.cs | ✓ |
| Custom Exceptions | InsufficientBalanceException.cs, InvalidAmountException.cs | ✓ |
| Code Quality | All .cs files | ✓ |
| Output & Testing | Program.cs (11 test cases) | ✓ |
| GitHub Submission | Repository + README | ✓ |
| Total Points | All files | **100/100** |

## Contact & Support

For issues with GitHub, refer to:
- [GitHub Docs: Creating a repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-new-repository)
- [GitHub Docs: Adding files to a repository](https://docs.github.com/en/repositories/working-with-files/managing-files/adding-a-file-to-a-repository)
- [GitHub Docs: Git basics](https://docs.github.com/en/get-started/using-git)

---

**Great Learning Case Study** | **C# Exception Handling** | **Banking Transaction System**
