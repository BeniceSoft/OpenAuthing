namespace BeniceSoft.OpenAuthing.Dtos
{
    public class KvContainer<TName, TValue>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public TName Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KvContainer : KvContainer<string, string>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class KvContainer<T> : KvContainer<string, T>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class ReadySelectDto : ReadySelectDto<Guid>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class ReadySelectDto<TKey>
    {
        /// <summary>
        /// Id
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class IdValueDto<TKey>
    {
        public IdValueDto(TKey id, string value)
        {
            Id = id;
            Value = value;
        }

        public IdValueDto()
        {
        }

        /// <summary>
        /// Id
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Value { get; set; }
    }
}