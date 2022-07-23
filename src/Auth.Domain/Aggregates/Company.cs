using System.Security.AccessControl;
using Auth.Domain.Enums;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Aggregates;

public class Company : IEntityBase<long>
{
    public Company() { }

    public Company(long id, string name, Location location)
    {
        Id = id;
        Name = name;
        Latitude = location.latitude;
        Longitude = location.longitude;
        Address = location.address;
        Departments = new List<Department>();
        Schedules = new List<Schedule>();
        Shifts = new List<Shift>();
        PunchSettings = new List<PunchGroup>();
        Users = new List<User>();
    }
    
    /// <summary>
    /// 公司 ID
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 公司名稱
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 緯度
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// 經度
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// 完整地址
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// 部門
    /// </summary>
    public virtual ICollection<Department> Departments { get; private set; }

    /// <summary>
    /// 工作表
    /// </summary>
    public virtual ICollection<Schedule> Schedules { get; private set; }

    /// <summary>
    /// 輪班
    /// </summary>
    public virtual ICollection<Shift> Shifts { get; private set; }

    /// <summary>
    /// 打卡群組
    /// </summary>
    public virtual ICollection<PunchGroup> PunchSettings { get; private set; }

    /// <summary>
    /// 公司人員
    /// </summary>
    public virtual ICollection<User> Users { get; set; }

    /// <summary>
    ///  加入使用者 - 最大權限
    /// </summary>
    /// <param name="user"></param>
    public void AddUserAsAdmin(User user)
    {
        user.Permission = Permission.admin;
        Users.Add(user);
    }
    
    /// <summary>
    /// 加入使用者 - 後台權限
    /// </summary>
    /// <param name="user"></param>
    public void AddUserAsBackstageManager(User user)
    {
        user.Permission = Permission.backstage;
        Users.Add(user);
    }

    /// <summary>
    ///  加入使用者 - 主管權限
    /// </summary>
    /// <param name="user"></param>
    public void AddUserAsManager(User user)
    {
        user.Permission = Permission.manager;
        Users.Add(user);
    }

    /// <summary>
    ///  加入使用者 - 員工權限
    /// </summary>
    /// <param name="user"></param>
    public void AddUserAsEmployee(User user)
    {
        user.Permission = Permission.employee;
        Users.Add(user);
    }
    
    /// <summary>
    /// 新增部門
    /// </summary>
    /// <param name="department"></param>
    public void AddDepartment(Department department) => Departments.Add(department);

    /// <summary>
    /// 新增工作表
    /// </summary>
    /// <param name="schedule"></param>
    public void AddSchedule(Schedule schedule) => Schedules.Add(schedule);

    /// <summary>
    /// 新增輪班
    /// </summary>
    /// <param name="shift"></param>
    public void AddShift(Shift shift) => Shifts.Add(shift);

    /// <summary>
    /// 新增打卡群組
    /// </summary>
    /// <param name="punchGroup"></param>
    public void AddPunchGroup(PunchGroup punchGroup) => PunchSettings.Add(punchGroup);
}